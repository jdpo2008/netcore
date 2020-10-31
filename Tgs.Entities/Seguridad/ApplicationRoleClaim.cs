using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tgs.Entities.Seguridad
{
    public class ApplicationRoleClaim : IdentityRoleClaim<int>
    {
        public virtual ApplicationRole Role { get; set; }
    }
}
