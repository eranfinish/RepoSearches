using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi.Controllers.Auth;

namespace AdApp.Core.Helpers
{
    public class AuthorizationMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthorizationMiddleware> _logger;

        public AuthorizationMiddleware(ILogger<AuthorizationMiddleware> logger, RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
            _logger = logger;
        }

        //public async Task Invoke(HttpContext context)
        //{
        //    if (context.Request.Path.ToString().Contains("/login") || 
        //        context.Request.Path.ToString().Contains("/register"))
        //    {
        //        await _next(context);
        //        return;
        //    }
           
        //    var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

          
        //    if (string.IsNullOrEmpty(token))
        //    {
        //        token = context.Request.Cookies["token"];
        //    }

        //    if (!string.IsNullOrEmpty(token))
        //    {
        //        if (ValidateToken(token, context))
        //        {
        //            await _next(context); // Continue down the pipeline if the token is valid
        //        }
        //        else
        //        {
        //            context.Response.StatusCode = StatusCodes.Status401Unauthorized; // Unauthorized
        //            await context.Response.WriteAsync("Invalid Token");
        //        }
        //    }
        //    else
        //    {
        //        // No token found
        //        context.Response.StatusCode = StatusCodes.Status401Unauthorized; // Unauthorized
        //        await context.Response.WriteAsync("Token is required");
        //    }
        //}

        public async Task Invoke(HttpContext context)
        {
            // Add logging
      //      _logger.LogInformation("Request Path: {Path}", context.Request.Path);

            // Log all cookies
            foreach (var cookie in context.Request.Cookies)
            {
                _logger.LogInformation("Found cookie: {Key}", cookie.Key);
            }

            if (context.Request.Path.ToString().Contains("/login") ||
                context.Request.Path.ToString().Contains("/register"))
            {
                await _next(context);
                return;
            }

            // Try Authorization header first
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            // If no Authorization header, try your specific cookie name
            if (string.IsNullOrEmpty(token))
            {
                token = context.Request.Cookies["token"]; // This matches your cookie name
                _logger.LogInformation("Cookie token value: {Token}", token ?? "null");
            }

            if (!string.IsNullOrEmpty(token))
            {
                _logger.LogInformation("Token found, validating...");

                if (ValidateToken(token, context))
                {
                    _logger.LogInformation("Token validation successful");
                    await _next(context);
                }
                else
                {
                    _logger.LogWarning("Token validation failed");
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Invalid Token");
                }
            }
            else
            {
                _logger.LogWarning("No token found in request");
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Token is required");
            }
        }
        private bool ValidateToken(string token, HttpContext context)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _configuration["Jwt:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero  // Remove default clock skew
                }, out SecurityToken validatedToken);

                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true, // Validate the signature of the token
                    IssuerSigningKey = new SymmetricSecurityKey(key), // Secret key for validation
                    ValidateIssuer = false, // Skip Issuer validation (set to true if needed)
                    ValidateAudience = false, // Skip Audience validation (set to true if needed)
                    ClockSkew = TimeSpan.Zero // No clock skew for expiration validation
                };
                
                // Validate the token and return claims
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out validatedToken);
                if (validatedToken is JwtSecurityToken jwtToken &&
              jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    var userId = principal.Claims.Select(x => x.Type == "id").FirstOrDefault();
                    context.Items.Add("userName",principal.Identity.Name);
                }
                    // Additional custom validation logic can go here

                    return true; // Return true if token is valid
            }
            catch
            {
                return false; // Return false if validation fails
            }

        }
    }

}
