using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tgs.Common.Util.General
{
    public class Constants
    {
        public static string SecretKey => "asdwda1d8a4sd8w4das8d*w8d*asd@#";

        public static string Audience => "http://localhost51324";

        public static string Issuer => "http://localhost51324";

        public static int Lifetime => 1;

        public static  bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken token, TokenValidationParameters @params)
        {
            if (expires != null)
            {
                return expires > DateTime.UtcNow;
            }
            return false;
        }
    }
   
}
