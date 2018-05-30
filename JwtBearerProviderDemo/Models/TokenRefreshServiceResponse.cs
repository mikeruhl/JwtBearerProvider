using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtBearerProviderDemo.Models
{
    public class TokenRefreshServiceResponse
    {
        public bool Authenticated { get; set; }

        public string UserId { get; set; }
    }
}
