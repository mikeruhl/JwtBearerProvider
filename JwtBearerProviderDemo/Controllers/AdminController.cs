using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtBearerProviderDemo.Controllers
{
    [Authorize(Roles = "admin", AuthenticationSchemes = "Bearer")]
    [Route("api/admin")]
    public class AdminController : Controller
    {
        public IActionResult Get()
        {
            return Ok("you're an admin!");
        }
    }
}
