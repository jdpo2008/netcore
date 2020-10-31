using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Tgs.DataAccess.Core.Context;
using Tgs.Entities.Seguridad;


namespace Tgs.Web.UI
{
    public static class SeedData
    {
        
        public static void EnsureSeedData(IServiceProvider serviceProvider)
        {
            Log.Information("Seed Data in DataBase");

            using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
                var RoleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
                var UserManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                context.Database.Migrate();

                IdentityResult roleResult;

                if (!RoleManager.RoleExistsAsync("USER").Result)
                {
                    var role = new ApplicationRole { Description = "Usaurio Invitado", Name = "USER", CreatedAt = DateTime.Now };
                    roleResult = RoleManager.CreateAsync(role).Result;
                }

                if (!RoleManager.RoleExistsAsync("ADMIN").Result)
                {
                    var role = new ApplicationRole { Description = "Administrador Sencillo", Name = "ADMIN", CreatedAt = DateTime.Now };
                    roleResult = RoleManager.CreateAsync(role).Result;
                }

                if (!RoleManager.RoleExistsAsync("SUPER_ADMIN").Result)
                {
                    var role = new ApplicationRole { Description = "Super Administrador", Name = "SUPER_ADMIN", CreatedAt = DateTime.Now };
                    roleResult = RoleManager.CreateAsync(role).Result;
                }

                if (UserManager.FindByEmailAsync("jdpo2008@gmail.com").Result == null)
                {
                    ApplicationUser user = new ApplicationUser();
                    user.UserName = "jdpo2008";
                    user.Email = "jdpo2008@gmail.com";
                    user.FirstName = "JOSE";
                    user.LastName = "PEREZ";
                    user.EmailConfirmed = true;
                    user.PhoneNumberConfirmed = true;
                    user.CreatedAt = DateTime.Now;

                    IdentityResult result;

                    result = UserManager.CreateAsync(user, "V3r0n1c@0304").Result;

                    if (result.Succeeded)
                    {
                        result = UserManager.AddToRoleAsync(user, "SUPER_ADMIN").Result;

                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = UserManager.AddClaimsAsync(user, new Claim[]{
                                new Claim(JwtClaimTypes.Name, user.FirstName),
                                new Claim(JwtClaimTypes.Email, user.Email),
                                new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                                new Claim(JwtClaimTypes.Role, "SUPER_ADMIN"),
                                new Claim(JwtClaimTypes.NickName, user.UserName)
                            }).Result;

                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        Log.Information("Usuario registrado correctamente " + user.Email);
                        Console.WriteLine("Usuario registrado correctamente " + user.Email);

                    }
                    else
                    {
                        Log.Fatal("Error en la BD" + result.Errors.First().Description);
                        throw new Exception(result.Errors.First().Description);
                    }
                }
                else
                {
                    Log.Information("Usuario ya se encuentra registrado" );
                    Console.WriteLine("Usuario ya se encuentra registrado");
                }
            }

        }
    }
}