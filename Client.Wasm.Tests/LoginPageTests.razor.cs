using Bunit;
using Bunit.TestDoubles;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using RichardSzalay.MockHttp;
using System.Net.Http;
using System.Text.Json;
using Blazored.LocalStorage;
using Syncfusion.Blazor;
using Client.Wasm.Pages;
using Client.Wasm.Services;
using Client.Wasm.DTOs;

namespace Client.Wasm.Tests;

public class LoginPageTests : TestContext
{
    [Fact]
    public void Login_ShowsError_WhenPasswordMissing()
    {
        var handler = new MockHttpMessageHandler();
        var http = handler.ToHttpClient();
        var storageMock = new Mock<ILocalStorageService>();
        var logger = Microsoft.Extensions.Logging.Abstractions.NullLogger<CustomAuthStateProvider>.Instance;
        var authService = new AuthService(http, new CustomAuthStateProvider(storageMock.Object, http, logger));
        Services.AddSingleton(authService);
        Services.AddSingleton<IAuthService>(authService);
        Services.AddSingleton<AuthenticationStateProvider>(new TestAuthenticationStateProvider());
        Services.AddSyncfusionBlazor();
        Services.AddSingleton<FakeNavigationManager>();
        Services.AddSingleton<NavigationManager>(sp => sp.GetRequiredService<FakeNavigationManager>());

        var cut = RenderComponent<Login>();

        cut.Find("form").Submit();

        Assert.Contains("Password", cut.Markup, System.StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Login_NavigatesHome_WhenSuccess()
    {
        var handler = new MockHttpMessageHandler();
        handler.When(HttpMethod.Post, "http://localhost/api/auth/login")
            .Respond("application/json", JsonSerializer.Serialize(new AuthResultDto { Token = "jwt", Expiration = DateTime.UtcNow }));
        var http = handler.ToHttpClient();
        http.BaseAddress = new Uri("http://localhost");
        var storageMock = new Mock<ILocalStorageService>();
        var logger = Microsoft.Extensions.Logging.Abstractions.NullLogger<CustomAuthStateProvider>.Instance;
        var authService = new AuthService(http, new CustomAuthStateProvider(storageMock.Object, http, logger));
        Services.AddSingleton(authService);
        Services.AddSingleton<IAuthService>(authService);
        Services.AddSingleton<AuthenticationStateProvider>(new TestAuthenticationStateProvider());
        Services.AddSyncfusionBlazor();
        Services.AddSingleton<FakeNavigationManager>();
        Services.AddSingleton<NavigationManager>(sp => sp.GetRequiredService<FakeNavigationManager>());

        var nav = Services.GetRequiredService<FakeNavigationManager>();
        var cut = RenderComponent<Login>();

        cut.Find("input[placeholder='Эл. почта']").Change("a@a.com");
        cut.Find("input[placeholder='Пароль']").Change("123456");
        cut.Find("form").Submit();

        Assert.Equal("http://localhost/", nav.Uri);
    }
}

class TestAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly AuthenticationState _state = new(new System.Security.Claims.ClaimsPrincipal());
    public override Task<AuthenticationState> GetAuthenticationStateAsync() => Task.FromResult(_state);
}
