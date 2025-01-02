using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace RepoSearches.JwtHandler
{
    // Middleware class - can be internal or public, depending on your use case
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly TokenValidationParameters _tokenValidationParameters;

        public JwtMiddleware(RequestDelegate next, TokenValidationParameters tokenValidationParameters)
        {
            _next = next;
            _tokenValidationParameters = tokenValidationParameters;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(token))
            {
                token = context.Request.Cookies["token"];
            }

            if (!string.IsNullOrEmpty(token))
            {
                try
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out var validatedToken);
                    if (validatedToken is JwtSecurityToken jwtToken &&
                        jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                    {
                        context.User = principal;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Token validation failed: {ex.Message}");
                }
            }

            await _next(context);
        }
    }

}
