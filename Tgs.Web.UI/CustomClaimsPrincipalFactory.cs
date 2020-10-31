using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Tgs.Entities.Seguridad;

namespace Tgs.Web.UI
{
    public class CustomClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, ApplicationRole> //: IUserClaimsPrincipalFactory<ApplicationUser>
    {
        public CustomClaimsPrincipalFactory(
          UserManager<ApplicationUser> userManager,
          RoleManager<ApplicationRole> roleManager,
          IOptions<IdentityOptions> optionsAccessor) : base(userManager, roleManager, optionsAccessor)
        {

        }

        public async override Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
        {
            var principal = await base.CreateAsync(user);
            var identity = (ClaimsIdentity)principal.Identity;

            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Name, user.FirstName ?? ""),
                new Claim(JwtClaimTypes.Email, user.Email ?? ""),
                new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                new Claim(JwtClaimTypes.NickName, user.UserName ?? "")
            };

            if (user.IsAdmin)
            {
                claims.Add(new Claim(JwtClaimTypes.Role, "SUPER_ADMIN"));
            }
            else
            {
                claims.Add(new Claim(JwtClaimTypes.Role, "USER"));
            }

            identity.AddClaims(claims);

            return principal;

        }
    }
}
