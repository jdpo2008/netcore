using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tgs.Entities.Seguridad
{
    public class ApplicationUserLogin : IdentityUserLogin<int>
    {
        public virtual ApplicationUser User { get; set; }
    }
}
