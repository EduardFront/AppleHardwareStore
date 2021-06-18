using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppleHardwareStore.Models
{
    public class Auth
    {
        // токен дается
        public const string ISSUER = "StoreApp"; 
        // токен забирается
        public const string AUDIENCE = "StoreAppClient"; 
        // ключ шифровальный
        const string KEY = "sndndsplsmbbfrrsprnc";
        // время жизни токена (минуты)
        public const int LIFETIME = 60; 
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
