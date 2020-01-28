using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoAPI.Models
{
    public class AuthResponse
    {
        public string Token { get; set; }

        public double ExpiresIn { get; set; }

        public AuthResponse(string token, DateTimeOffset expiration)
        {
            Token = token;
            DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan elapsedTime = expiration - unixEpoch;
            ExpiresIn = elapsedTime.TotalMilliseconds;
        }
    }
}
