using Blazored.Toast;
using CompanyName.Web;
using CompanyName.Web.ApiServiceClients;
using CompanyName.Web.Constants;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMsalAuthentication(options =>
{
    builder.Configuration.Bind("AzureAdB2C", options.ProviderOptions.Authentication);
    //options.ProviderOptions.Cache.CacheLocation = "localStorage";
    options.ProviderOptions.LoginMode = "redirect";

    var apiScope = builder.Configuration.GetValue<string>(AppSettingsConstants.ApiScope);
    if (apiScope != null)
    {
        options.ProviderOptions.DefaultAccessTokenScopes.Add(apiScope);
    }
});

builder.Services.AddHttpClient<DepartmentServiceClient>((sp, c) =>
{
    var apiBaseUrl = builder.Configuration.GetValue<string>(AppSettingsConstants.ApiBaseUrl);
    if (apiBaseUrl == null)
    {
        throw new Exception("ApiBaseUrl is not defined in the appsettings.json");
    }
    c.BaseAddress = new Uri(apiBaseUrl);
}).AddHttpMessageHandler<HttpAuthorizationMessageHandler>();

builder.Services.AddHttpClient<EmployeeServiceClient>((sp, c) =>
{
    var apiBaseUrl = builder.Configuration.GetValue<string>(AppSettingsConstants.ApiBaseUrl);
    if (apiBaseUrl == null)
    {
        throw new Exception("ApiBaseUrl is not defined in the appsettings.json");
    }
    c.BaseAddress = new Uri(apiBaseUrl);
}).AddHttpMessageHandler<HttpAuthorizationMessageHandler>();

builder.Services.AddScoped<HttpAuthorizationMessageHandler>();
builder.Services.AddHttpClient();
builder.Services.AddBlazoredToast();

await builder.Build().RunAsync();
