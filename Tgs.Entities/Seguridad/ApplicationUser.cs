using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tgs.Entities.Seguridad
{
    public class ApplicationUser : IdentityUser<int>
    {
        [PersonalData]
        public string FirstName { get; set; }
        [PersonalData]
        public string LastName { get; set; }
        [PersonalData]
        public string CustomTag { get; set; }
        [PersonalData]
        public int UserCreated { get; set; }
        [PersonalData]
        public DateTime CreatedAt { get; set; }
        [PersonalData]
        public DateTime? UpdateAt { get; set; }
        public bool IsAdmin { get; set; }
        public string DataEventRecordsRole { get; set; }
        public string SecuredFilesRole { get; set; }

        public virtual ICollection<ApplicationUserClaim> Claims { get; set; }
        public virtual ICollection<ApplicationUserLogin> Logins { get; set; }
        public virtual ICollection<ApplicationUserToken> Tokens { get; set; }
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}
