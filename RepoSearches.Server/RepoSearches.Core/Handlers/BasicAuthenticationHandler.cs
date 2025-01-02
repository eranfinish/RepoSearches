using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using RepoSearches.Core.Services.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Owin.Logging;
 
namespace RepoSearches.Core.Handlers
{
   

    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private IAuthService _authService;
        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            Microsoft.Extensions.Logging.ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock, IAuthService authService)
            : base(options, (Microsoft.Extensions.Logging.ILoggerFactory)logger, encoder, clock)
        {
            _authService = authService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Missing Authorization Header");

            var authHeader = Request.Headers["Authorization"].ToString();
            if (!authHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
                return AuthenticateResult.Fail("Invalid Authorization Header");

            var token = authHeader.Substring("Basic ".Length).Trim();
            var credentialString = Encoding.UTF8.GetString(Convert.FromBase64String(token));
            var credentials = credentialString.Split(':');

            if (credentials.Length != 2)
                return AuthenticateResult.Fail("Invalid Authorization Header");

            var username = credentials[0];
            var password = credentials[1];

            // Here you would check the credentials against your database or membership provider
            // For example:
            if (_authService.AuthorizeUser(username, password)) // Replace with real authentication logic
            {
                var claims = new[] { new Claim(ClaimTypes.Name, username) };
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return AuthenticateResult.Success(ticket);
            }

            return AuthenticateResult.Fail("Invalid Username or Password");
        }
    }
}
