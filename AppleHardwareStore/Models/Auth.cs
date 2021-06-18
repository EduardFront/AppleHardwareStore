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
        public const string ISSUER = "StoreApp"; // издатель токена
        public const string AUDIENCE = "StoreAppClient"; // потребитель токена
        const string KEY = "sndndsplsmbbfrrsprnc";   // ключ для шифрации
        public const int LIFETIME = 60; // время жизни токена в минутах
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
