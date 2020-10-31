using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;

namespace Tgs.DataContract.Identity
{
    public class UserModelDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public List<Claim> Roles { get; set; }
        public bool EmailConfirmed { get; set; }
        public string JwtToken { get; set; }
    }
}
