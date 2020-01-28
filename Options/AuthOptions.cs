using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace DemoAPI.Options
{
    public class AuthOptions
    {
        public string Key { get; set; } = string.Empty; // ключ для шифрации
        public int ExpiresIn { get; set; } = 1; // время жизни токена - устанавливать в конфиге

        public SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
        }
    }
}