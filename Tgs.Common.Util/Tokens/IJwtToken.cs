using System;
using System.Collections.Generic;
using System.Text;
using Tgs.Entities.Seguridad;

namespace Tgs.Common.Util.Tokens
{
    public interface IJwtToken
    {
        string CreateToken(ApplicationUser user);
    }
}
