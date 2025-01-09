using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using BCrypt.Net;
using RepoSearches.DAL;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RepoSearches.Models.DTOs;
using RepoSearches.Models.Entities;
using RepoSearches.Models;
using RepoSearches.Core.Services.Auth;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;

namespace RepoSearches.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IOptions<JwtSettings> _jwtSettings;
        private readonly ILogger<AuthController> _logger;
        private readonly TokenValidationParameters _tokenValidationParameters;

        public AuthController(IAuthService authService, 
            IOptions<JwtSettings> jwtSettings,
            ILogger<AuthController> logger, 
            TokenValidationParameters tokenValidationParameters
            )
        {
            _authService = authService;
            _jwtSettings = jwtSettings;
            _logger = logger;
            _tokenValidationParameters = tokenValidationParameters;
        }
                
        [HttpGet("check-auth")]
        public IActionResult CheckAuthStatus()
        {
            try
            {
                _logger.LogInformation("User Identity: {Identity}", User.Identity);
                _logger.LogInformation("Is Authenticated: {IsAuthenticated}", User.Identity?.IsAuthenticated);
                _logger.LogInformation("Auth Type: {AuthType}", User.Identity?.AuthenticationType);
                _logger.LogInformation("Claims: {Claims}", string.Join(", ", User.Claims.Select(c => $"{c.Type}={c.Value}")));

                if (User.Identity?.IsAuthenticated == true)
                {
                    return Ok(new
                    {
                        isAuthenticated = true,
                        username = User.Identity.Name
                    });
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {

                //simulate Log exception
                Console.WriteLine(ex.Message);

                return BadRequest(ex.Message);
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto registerDto)
        {
            try
            {


                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var token = await _authService.RegisterAsync(registerDto);
                if (token == string.Empty)
                    return BadRequest("Username or email already exists");

                // Set the token as an HTTP-only cookie
                AppendAuthToken(token);

                return Ok(new { message = "User registered successfully" });
            }
            catch (Exception ex)
            {
                //simulate Log exception
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        private void AppendAuthToken(string token)
        {
            _logger.LogInformation("Setting auth token cookie. Token length: {Length}", token.Length);
            Response.Cookies.Append("token", token, new CookieOptions
            {
                HttpOnly = false,  // Must be false to be readable by JavaScript
                Secure = true,
                Expires = DateTime.UtcNow.AddHours(24),
                SameSite = SameSiteMode.None,
                Domain = "localhost",
                Path = "/"
            });
            Response.Headers.Add("X-Debug-Token", "Token-Set");
            //   Response.Cookies.Append("token", token, cookieOptions);
            //_logger.LogInformation("Cookie set with options: {@Options}", cookieOptions);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {


                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var user = await _authService.LoginAsync(loginDto);
              
                if (user.JwtToken == null)
                    return Unauthorized("Invalid credentials");

                // Set the token as an HTTP-only cookie
                AppendAuthToken(user.JwtToken);

               
                user.JwtToken = string.Empty;
                user.Password = string.Empty;
               
                
                return Ok(user);
            }
            catch (Exception ex)
            {   //simulate Log exception
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // Clear the authentication cookie by setting it to expire
            Response.Cookies.Append("token", "", new CookieOptions
            {
                HttpOnly = false,
                Secure = true, // Ensure this matches your environment
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(-1) // Set to a past date
            });

            return Ok(new { message = "Logged out successfully" });
        }

    }
}
