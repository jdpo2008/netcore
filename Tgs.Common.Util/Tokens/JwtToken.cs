using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Tgs.Entities.Seguridad;
using Tgs.Common.Util.General;
using Microsoft.Extensions.Logging;

namespace Tgs.Common.Util.Tokens
{
    public class JwtToken : IJwtToken
    {
        private readonly ILogger<JwtToken> _logger;

        public JwtToken(ILogger<JwtToken> logger)
        {
            this._logger = logger;
        }

        public string CreateToken(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Email )
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Constants.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = Constants.Issuer,
                Audience = Constants.Audience,
                Expires = DateTime.Now.AddHours(Constants.Lifetime),
                SigningCredentials = creds,
                NotBefore = DateTime.Now
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}
