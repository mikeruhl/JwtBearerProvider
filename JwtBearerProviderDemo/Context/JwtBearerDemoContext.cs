using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JwtBearerProviderDemo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JwtBearerProviderDemo.Context
{
    public class JwtBearerDemoContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public JwtBearerDemoContext(DbContextOptions<JwtBearerDemoContext> options)
            : base(options)
        { }
    }
}
