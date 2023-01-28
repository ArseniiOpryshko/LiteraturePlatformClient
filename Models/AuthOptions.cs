using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace LiteraturePlatformClient.Models
{
    public class AuthOptions
    {
        public const string ISSUER = "LitPlatform"; // издатель токена
        public const string AUDIENCE = "LitPlatform"; // потребитель токена
        const string KEY = "phahwawaeewewedsdkey";   // ключ для шифрации
        public const int LIFETIME = 10; // время жизни токена 
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
