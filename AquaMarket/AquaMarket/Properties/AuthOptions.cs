using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaServer.Properties
{
    internal static class AuthOptions
    {
        public const string ISSUER = nameof(AquaServer); // издатель токена
        public const string AUDIENCE = nameof(AquaServer); 
        const string KEY = "AC3E457D-CDC4-42F6-8153-633BE25ED512";   // ключ для шифрации
        public static int LIFETIME = (int)TimeSpan.FromDays(3).TotalMinutes; // время жизни токена 
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
