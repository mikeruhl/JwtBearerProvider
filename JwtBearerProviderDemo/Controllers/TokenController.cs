using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using JwtBearerProviderDemo.JwtProvider;
using JwtBearerProviderDemo.Models;
using JwtBearerProviderDemo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
namespace JwtBearerProviderDemo.Controllers
{
    [Route("token")]
    public class TokenController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signinManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TokenProviderOptions _options;
        private readonly TokenRefreshService _tokenRefreshService;

        public TokenController(SignInManager<ApplicationUser> signinManager, 
            UserManager<ApplicationUser> userManager, 
            IOptions<TokenProviderOptions> options,
            TokenRefreshService tokenRefreshService)
        {
            _tokenRefreshService = tokenRefreshService;
            _options = options.Value;
            _userManager = userManager;
            _signinManager = signinManager;
        }

        /// <summary>
        /// Token authentication endpoint used to retrieve access and refresh tokens
        /// </summary>
        /// <param name="request">Credentials or tokens required to obtain access token</param>
        /// <returns>Upon successful authentication, returns authorization token</returns>
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(TokenResponse), 200)]
        [HttpPost]
        public async Task<IActionResult> GetToken([FromBody] TokenRequest request)
        {
            if (request == null)
                return BadRequest("Request body not recognized");

            switch (request.GrantType)
            {
                case "client_credentials":
                    return await AuthorizeClientAsync(request);
                case "refresh_token":
                    return await RefreshJwtTokenAsync(request);
                default:
                    return BadRequest("grant_type supplied is unsupported.");
            }
        }

        private async Task<IActionResult> RefreshJwtTokenAsync(TokenRequest request)
        {
            if (string.IsNullOrEmpty(request.RefreshToken))
                return BadRequest("Request token not supplied.");


            var tokenResponse = _tokenRefreshService.ValidateRefreshToken(request.RefreshToken);

            if (tokenResponse.Authenticated)
            {
                var user = await _userManager.FindByIdAsync(tokenResponse.UserId.ToString());

                if (user == null)
                    return Unauthorized();

                var principal = await _signinManager.CreateUserPrincipalAsync(user);


                if (principal != null)
                    return await IssueJwtAsync(principal.Identities.First());
            }

            return Unauthorized();
        }

        private async Task<IActionResult> AuthorizeClientAsync(TokenRequest request)
        {
            if (string.IsNullOrEmpty(request.ClientId) || string.IsNullOrEmpty(request.ClientSecret))
                return BadRequest("username and password must be provided.");

            var identity = await AuthenticateAsync(request.ClientId, request.ClientSecret);
            if (identity == null)
            {
                return Unauthorized();
            }

            return await IssueJwtAsync(identity);
        }

        private async Task<IActionResult> IssueJwtAsync(ClaimsIdentity identity)
        {
            var now = DateTimeOffset.Now;

            var userId = identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            var refreshToken = _tokenRefreshService.GetRefreshToken(userId);

            // Create the JWT and write it to a string
            var jwt = new JwtSecurityToken(
                _options.Issuer,
                _options.Audience,
                identity.Claims,
                now.DateTime,
                now.DateTime.Add(_options.Expiration),
                _options.SigningCredentials);
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new TokenResponse
            {
                AccessToken = encodedJwt,
                ExpiresIn = (int)_options.Expiration.TotalSeconds,
                RefreshToken = refreshToken
            };

            // Serialize and return the response
            return Json(response);
        }

        private async Task<ClaimsIdentity> AuthenticateAsync(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
                return null;

            var passwordSignInResult = await _signinManager.CheckPasswordSignInAsync(user, password, false);

            if (!passwordSignInResult.Succeeded)
            {
                return null;
            }

            var principal = await _signinManager.CreateUserPrincipalAsync(user);

            return principal.Identities.First();

        }
    }
}
