using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using Client.Wasm;
using Client.Wasm.Services;
using MudBlazor.Services;

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

builder.Services.AddHttpClient<IAgentApiClient, AgentApiClient>(client =>
{
    client.BaseAddress = new Uri("http://localhost:8000/");
});

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddMudServices();

builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<CustomAuthStateProvider>());
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddSingleton<MenuService>();

var assembly = typeof(Program).Assembly;
var clients = assembly.GetTypes().Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("ApiClient"));
foreach (var client in clients)
{
    if (client == typeof(AgentApiClient))
        continue;
    var iface = client.GetInterface($"I{client.Name}");
    if (iface != null)
    {
        builder.Services.AddScoped(iface, client);
    }
}

await builder.Build().RunAsync();
