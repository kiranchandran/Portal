using Microsoft.AspNetCore.Authentication;

namespace CompanyName.Api.IntegrationTests.Fakes
{
    public class TestAuthHandlerOptions : AuthenticationSchemeOptions
    {
        public string DefaultUserId { get; set; } = null!;
    }
}
