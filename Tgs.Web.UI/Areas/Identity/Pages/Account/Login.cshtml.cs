using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Tgs.Entities.Seguridad;
using Tgs.DataContract.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Cors;
using LazZiya.ExpressLocalization.TagHelpers;
using LazZiya.ExpressLocalization;

namespace Tgs.Web.UI.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    [EnableCors("AllowsAll")]
    public class LoginModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly IConfiguration _configuration;
        private readonly ISharedCultureLocalizer _loc;
        private readonly string culture;

        public LoginModel(SignInManager<ApplicationUser> signInManager, 
            ILogger<LoginModel> logger,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
            ISharedCultureLocalizer loc)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _configuration = configuration;
            _loc = loc;
            culture = System.Globalization.CultureInfo.CurrentCulture.Name;
        }

        [BindProperty]
        public LoginModelDto Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                try
                {
                    var user = await _userManager.FindByEmailAsync(Input.Email);

                    var result = await _signInManager.PasswordSignInAsync(user.UserName, Input.Password, Input.RememberMe, lockoutOnFailure: false);

                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User logged in.");

                        return LocalRedirect(returnUrl);
                    }
                    if (result.RequiresTwoFactor)
                    {
                        return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                    }
                    if (result.IsLockedOut)
                    {
                        _logger.LogWarning("User account locked out.");
                        return RedirectToPage("./Lockout");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Debes verificar el Email antes de iniciar sesión.");
                        return Page();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error al ingresar " + ex.Message);
                }

                
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private String GetToken(ApplicationUser user)
        {
            var utcNow = DateTime.UtcNow;

            var claims = new Claim[]
            {
                 new Claim(JwtRegisteredClaimNames.Email, user.Email),
                 new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                 new Claim(JwtRegisteredClaimNames.Iat, utcNow.ToString()),

            };

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<String>("Tokens:Key")));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var jwt = new JwtSecurityToken(
                signingCredentials: signingCredentials,
                claims: claims,
                notBefore: utcNow,
                expires: utcNow.AddMinutes(_configuration.GetValue<int>("Tokens:Lifetime")),
                audience: _configuration.GetValue<String>("Tokens:Audience"),
                issuer: _configuration.GetValue<String>("Tokens:Issuer")
                );

            return new JwtSecurityTokenHandler().WriteToken(jwt);

        }
    }
}
