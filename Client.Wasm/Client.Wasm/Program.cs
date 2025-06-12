using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Syncfusion.Blazor;
using Client.Wasm;
using Client.Wasm.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddSyncfusionBlazor();
builder.Services.AddScoped<ProtectedLocalStorage>();

builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<CustomAuthStateProvider>());

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
