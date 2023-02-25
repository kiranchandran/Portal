using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace CompanyName.Api.IntegrationTests.Fakes
{
    internal class TestAuthHandler : AuthenticationHandler<TestAuthHandlerOptions>
    {
        public const string UserId = "123456";

        public const string AuthenticationScheme = "Test";
        private readonly string _defaultUserId;

        public TestAuthHandler(
            IOptionsMonitor<TestAuthHandlerOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock) : base(options, logger, encoder, clock)
        {
            _defaultUserId = options.CurrentValue.DefaultUserId;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, "Test user") };

            // Extract User ID from the request headers if it exists,
            // otherwise use the default User ID from the options.
            if (Context.Request.Headers.TryGetValue(UserId, out var userId))
            {
                claims.Add(new Claim(ClaimTypes.NameIdentifier, userId[0]));
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.NameIdentifier, _defaultUserId));
            }

            claims.Add(new Claim(ClaimTypes.Email, "test@example.com"));
            claims.Add(new Claim("http://schemas.microsoft.com/identity/claims/scope", "API"));


            var identity = new ClaimsIdentity(claims, AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, AuthenticationScheme);

            var result = AuthenticateResult.Success(ticket);

            return Task.FromResult(result);
        }
    }
}
