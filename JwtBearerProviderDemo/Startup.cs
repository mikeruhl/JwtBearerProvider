using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using JwtBearerProviderDemo.Context;
using JwtBearerProviderDemo.JwtProvider;
using JwtBearerProviderDemo.Models;
using JwtBearerProviderDemo.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace JwtBearerProviderDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<JwtBearerDemoContext>(opt => opt.UseInMemoryDatabase("default"))
                ;
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<JwtBearerDemoContext>()
                .AddDefaultTokenProviders();

            //set these up as env vars
            var audience = "mike.mike";
            var issuer = "mike.mike.mike";
            var secretString = "SuperSecretPassword";

            services.Configure<TokenProviderOptions>(opt =>
            {
                opt.Audience = audience;
                opt.Issuer = issuer;
                opt.SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretString)), SecurityAlgorithms.HmacSha256);
                opt.Expiration = TimeSpan.FromSeconds(300);
            });

            // Services used by identity
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opt =>
            {

                var secretKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretString));

                var tokenValidationParameters = new TokenValidationParameters
                {

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = secretKey,
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                };
                opt.TokenValidationParameters = tokenValidationParameters;
                opt.SaveToken = true;
            });

            services.AddSingleton<TokenRefreshService>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
            var passwordHasher = serviceProvider.GetService<IPasswordHasher<ApplicationUser>>();
            var roleManager = serviceProvider.GetService<RoleManager<ApplicationRole>>();


            if (!userManager.Users.Any())
            {
                var regularUser = new ApplicationUser();
                regularUser.Email = "user@mike.mike";
                regularUser.UserName = "user";
                regularUser.PhoneNumber = "123-4567-7890";
                regularUser.PasswordHash = passwordHasher.HashPassword(regularUser, "password123");

                Task.WaitAll(Task.Run(async () =>
                {
                    await userManager.CreateAsync(regularUser);

                    await roleManager.CreateAsync(new ApplicationRole()
                    {
                        Name = "admin"
                    });

                    var adminUser = new ApplicationUser();
                    adminUser.Email = "admin@mike.mike";
                    adminUser.UserName = "AdminMaster";
                    adminUser.PhoneNumber = "987-654-3210";
                    adminUser.PasswordHash = passwordHasher.HashPassword(regularUser, "password123");

                    await userManager.CreateAsync(adminUser);

                    await userManager.AddClaimAsync(adminUser, new Claim(ClaimTypes.Role, "admin"));
                }));



            }

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
