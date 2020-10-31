using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Tgs.Entities.Seguridad;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Tgs.DataAccess.Core.Context;
using Serilog;
using System.Globalization;
using Tgs.Common.Util.LocalizationResources;
using LazZiya.ExpressLocalization;
using Microsoft.AspNetCore.Localization;
using LazZiya.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Tgs.Web.UI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region Add CORS  

            services.AddCors(options => options.AddPolicy("Cors", builder =>
            {
                builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            }));
            Log.Information("Se agregaron los CORS");
            #endregion

            #region Add Context
            string assemblyName = typeof(ApplicationDbContext).Namespace;
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("Tgs.DataAccess.Core")));
            Log.Information("Conectado a la Base de Datos");
            #endregion

            #region Add Entity Framework and Identity Framework  

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddRoles<ApplicationRole>()
                //.AddErrorDescriber<CustomIdentityErrorDescriber>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();

            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, CustomClaimsPrincipalFactory>();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 1;

                // Sing settings.
                options.SignIn.RequireConfirmedEmail = true;
                options.SignIn.RequireConfirmedAccount = true;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            });
            Log.Information("Se agrego el Identity en la aplicacion");
            #endregion

            #region Add Authentication  

            var key = Encoding.ASCII.GetBytes(Configuration.GetValue<string>("JwTokens:SecretKey"));

            //services.AddAuthentication(x =>
            //{
            //    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //}).AddJwtBearer(x =>
            //{
            //    x.RequireHttpsMetadata = false;
            //    x.SaveToken = true;
            //    x.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuerSigningKey = true,
            //        IssuerSigningKey = new SymmetricSecurityKey(key),
            //        ValidateAudience = true,
            //        ValidAudience = this.Configuration["Tokens:Audience"],
            //        ValidateIssuer = true,
            //        ValidIssuer = this.Configuration["Tokens:Issuer"],
            //        ValidateLifetime = true,
            //    };
            //});

            #endregion

            #region Add Authentication Providers

            services.AddAuthentication()
               .AddGoogle(options =>
               {
                   IConfigurationSection googleAuthNSection =
                       Configuration.GetSection("Authentication:Google");

                   options.ClientId = googleAuthNSection["ClientId"];
                   options.ClientSecret = googleAuthNSection["ClientSecret"];
               });

            #endregion

            #region Cookies Options
            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
            });

            services.AddTransient<ITagHelperComponent, LocalizationValidationScriptsTagHelperComponent>();
            #endregion

            var cultures = new[]
           {
                new CultureInfo("en-us"),
                new CultureInfo("es-es"),
                new CultureInfo("fr-fr"),
                new CultureInfo("pt-pt"),
                new CultureInfo("pt-br"),
                new CultureInfo("ru-ru"),
                new CultureInfo("zh-cn")
            };
            services.AddRazorPages()
                .AddExpressLocalization<ExpressLocalizationResource, ViewLocalizationResource>(ops =>
            {
                //ops.UseAllCultureProviders = false;
                ops.ResourcesPath = "LocalizationResources";
                ops.RequestLocalizationOptions = o =>
                {
                    o.SupportedCultures = cultures;
                    o.SupportedUICultures = cultures;
                    o.DefaultRequestCulture = new RequestCulture("en");
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider service)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseSerilogRequestLogging();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();

            app.UseCors(options => options.AllowAnyOrigin());
            app.UseCookiePolicy();

            app.UseRequestLocalization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });

            bool seed = Configuration.GetSection("Data").GetValue<bool>("Seed");
            if (seed)
            {
                SeedData.EnsureSeedData(service);
            }
        }
    }
}
