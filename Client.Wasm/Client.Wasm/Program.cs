using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using Client.Wasm;
using Client.Wasm.Services;
using Syncfusion.Blazor;
using Syncfusion.Licensing;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// HttpClient для API
builder.Services.AddScoped<ErrorHandlerService>();
builder.Services.AddScoped(sp =>
{
    var handler = new ErrorHandlingMessageHandler(sp.GetRequiredService<ErrorHandlerService>());
    return new HttpClient(handler) { BaseAddress = new Uri("http://localhost:5141/") };
});

// Регистрация клиентских сервисов
builder.Services.AddScoped<Client.Wasm.Services.AuthService>();
builder.Services.AddScoped<Client.Wasm.Services.IServiceApiClient, Client.Wasm.Services.ServiceApiClient>();
builder.Services.AddScoped<Client.Wasm.Services.IWorkflowApiClient, Client.Wasm.Services.WorkflowApiClient>();
builder.Services.AddScoped<Client.Wasm.Services.IApplicationApiClient, Client.Wasm.Services.ApplicationApiClient>();
builder.Services.AddScoped<Client.Wasm.Services.IDocumentApiClient, Client.Wasm.Services.DocumentApiClient>();
builder.Services.AddScoped<Client.Wasm.Services.IOrderApiClient, Client.Wasm.Services.OrderApiClient>();
builder.Services.AddScoped<Client.Wasm.Services.IOutgoingApiClient, Client.Wasm.Services.OutgoingApiClient>();
builder.Services.AddScoped<Client.Wasm.Services.IGeoApiClient, Client.Wasm.Services.GeoApiClient>();
builder.Services.AddScoped<Client.Wasm.Services.ITemplateApiClient, Client.Wasm.Services.TemplateApiClient>();
builder.Services.AddScoped<Client.Wasm.Services.IUserApiClient, Client.Wasm.Services.UserApiClient>();
builder.Services.AddScoped<Client.Wasm.Services.PreloaderService>();
builder.Services.AddScoped<Client.Wasm.Services.IDocumentTemplateService, Client.Wasm.Services.DocumentTemplateService>();
builder.Services.AddScoped<Client.Wasm.Services.DocumentGeneratorService>();

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddSyncfusionBlazor();
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mgo+DSMBaFt/QHJqVVhkXFpFdEBBXHxAd1p/VWJYdVt5flBPcDwsT3RfQF9jQX5XdkNmW31YeXRUTg==;Mgo+DSMBPh8sVXJ0S0R+XE9AdlRDX3xKf0x/TGpQb19xflBPallYVBYiSV9jS3xTfkVnWXlbcnZTT2FUWQ==;ORg4AjUWIQA/Gnt2VVhiQlFacl9JXGFWfVJpTGpQdk5xdV9DaVZUTWY/P1ZhSXxRdk1jXX9YdXZXQGlfVEw=;NjkwNjYzQDMyMzAyZTMyMmUzMEZNSTJGQXd0bjdkeVdBWXp2RFZSR1JqTVhycjJPNzE5YTFFdERnUjVWSXc9;NjkwNjY0QDMyMzAyZTMyMmUzMGg0RFJ2SnFhejkyTnVKdUMvZE1jYktjS3Z3UzRYWlkzZEVCVXZwNUQ4Yms9;NRAiBiAaIQQuGjN/V0Z+Xk9EaFtCVmJLYVB3WmpQdldgdVRMZVVbQX9PIiBoS35RdEVrWXtednFXRGdcUUB+;NjkwNjY2QDMyMzAyZTMyMmUzME83eCsrSDRZUndWN1hTczNDN2FadFlOU2grUWVxK0hvZ3F2a1hqNjRoZVU9;NjkwNjY3QDMyMzAyZTMyMmUzMFR5dmFJL3htU0FHbm9ZU3l4TFNPVGt1N0RzeFI2SHJiSWg4bHh6cmlPcWs9;Mgo+DSMBMAY9C3t2VVhiQlFacl9JXGFWfVJpTGpQdk5xdV9DaVZUTWY/P1ZhSXxRdk1jXX9YdXZXQWBaVEw=;NjkwNjY5QDMyMzAyZTMyMmUzMEl2TFBabW02MTZBZWhzM2N1bUhDU01JOTZNZThkU0JwNGlBck9pRlhFQ0E9;NjkwNjcwQDMyMzAyZTMyMmUzMFcwY3g0V1FzMEJMNmJ0R0lvVHMrY20xT0ZyUCs0RzdCRDlESW14Rkd2czA9;NjkwNjcxQDMyMzAyZTMyMmUzME83eCsrSDRZUndWN1hTczNDN2FadFlOU2grUWVxK0hvZ3F2a1hqNjRoZVU9");

builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<CustomAuthStateProvider>());
builder.Services.AddScoped<IAuthService, AuthService>();

var assembly = typeof(Program).Assembly;
var clients = assembly.GetTypes().Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("ApiClient"));
foreach (var client in clients)
{
    var iface = client.GetInterface($"I{client.Name}");
    if (iface != null)
    {
        builder.Services.AddScoped(iface, client);
    }
}

await builder.Build().RunAsync();
