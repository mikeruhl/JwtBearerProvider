using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace JwtBearerProviderDemo.Models
{
    public class TokenRequest
    {
        /// <summary>
        /// Grant types currently supported: client_credentials and refresh_token
        /// </summary>
        [Required]
        [JsonProperty("grant_type")]
        public string GrantType { get; set; }

        /// <summary>
        /// The username of the user (not required for grant_type: refresh_token)
        /// </summary>
        [JsonProperty("client_id")]
        public string ClientId { get; set; }

        /// <summary>
        /// The Password of the user (not required for grant_type: refresh_token)
        /// </summary>
        [JsonProperty("client_secret")]
        public string ClientSecret { get; set; }

        /// <summary>
        /// The refresh token used to obtain a new Jwt token
        /// </summary>
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
    }
}
