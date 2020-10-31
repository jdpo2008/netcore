using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tgs.Common.Util.Tokens;
using Tgs.DataContract.Identity;
using Tgs.Entities.Seguridad;

namespace Tgs.Web.Authentication.Api.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtToken _jwtToken;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IJwtToken jwtToken)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtToken = jwtToken;
        }

        
        [HttpPost("Login")]
        public async Task<ActionResult<UserModelDto>> Login([FromBody] LoginModelDto model)
        {

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
                throw new Exception("user authentication error ");

            var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, lockoutOnFailure: true);

            if (result.Succeeded)
            {
                return new UserModelDto
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    UserName = user.UserName,
                    EmailConfirmed = user.EmailConfirmed,
                    JwtToken = _jwtToken.CreateToken(user),
                };
            }

            throw new Exception("not authorize");
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("Test")]
        public ActionResult<string> Test()
        {
            return "For authentication testing.";
        }

    }
}
