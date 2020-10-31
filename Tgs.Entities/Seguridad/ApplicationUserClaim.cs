using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tgs.Entities.Seguridad
{
    public class ApplicationUserClaim : IdentityUserClaim<int>
    {
        public virtual ApplicationUser User { get; set; }
    }
}
