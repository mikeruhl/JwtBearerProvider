using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JwtBearerProviderDemo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JwtBearerProviderDemo.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/user")]
    public class UserController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser>  userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

            return Ok(user.UserName);
        }
    }
}
