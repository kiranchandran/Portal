using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components;
using CompanyName.Web.Constants;
using Blazored.Toast.Services;

namespace CompanyName.Web.ApiServiceClients
{
    public class HttpAuthorizationMessageHandler : AuthorizationMessageHandler
    {
        public HttpAuthorizationMessageHandler(IAccessTokenProvider provider,
            NavigationManager navigationManager, IConfiguration configuration)
            : base(provider, navigationManager)
        {
            var apiBaseUrl = configuration.GetValue<string>(AppSettingsConstants.ApiBaseUrl);
            ConfigureHandler(
               authorizedUrls: new[] { apiBaseUrl });

        }
    }
}
