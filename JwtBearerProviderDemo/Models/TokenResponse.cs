using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace JwtBearerProviderDemo.Models
{
    public class TokenResponse
    {
        /// <summary>
        /// Signed Jwt Token for authorizing requests.
        /// </summary>
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        /// Time to live for Jwt Token in seconds
        /// </summary>
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        /// <summary>
        /// Refresh token provided to retrieve new Jwt access token.
        /// </summary>
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

    }
}
