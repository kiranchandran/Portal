using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace CompanyName.Api.IntegrationTests.Fakes
{
    internal class FakeUserFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            context.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, "123"),
            new Claim(ClaimTypes.Name, "Test user"),
            new Claim(ClaimTypes.Email, "test@example.com"),
            new Claim("http://schemas.microsoft.com/identity/claims/scope", "API")
        }));

            await next();
        }
    }
}
