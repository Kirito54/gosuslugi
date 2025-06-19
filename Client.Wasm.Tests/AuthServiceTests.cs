using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using RichardSzalay.MockHttp;
using Moq;
using Xunit;
using Client.Wasm.Services;
using Client.Wasm.DTOs;

namespace Client.Wasm.Tests;

public class AuthServiceTests
{
    [Fact]
    public async Task LoginAsync_ReturnsTrue_WhenResponseOk()
    {
        var handler = new MockHttpMessageHandler();
        var dto = new LoginRequestDto { Email = "a@a.com", Password = "pass" };
        var result = new AuthResultDto { Token = "jwt", Expiration = DateTime.UtcNow.AddHours(1) };
        handler.When(HttpMethod.Post, "http://localhost/api/auth/login")
            .Respond("application/json", JsonSerializer.Serialize(result));

        var http = handler.ToHttpClient();
        http.BaseAddress = new Uri("http://localhost");

        var storageMock = new Mock<ILocalStorageService>();
        storageMock.Setup(s => s.SetItemAsync<string>("authToken", It.IsAny<string>()))
            .Returns(ValueTask.CompletedTask);

        var logger = Microsoft.Extensions.Logging.Abstractions.NullLogger<CustomAuthStateProvider>.Instance;
        var provider = new CustomAuthStateProvider(storageMock.Object, http, logger);
        var service = new AuthService(http, provider);

        var success = await service.LoginAsync(dto);

        Assert.True(success);
        storageMock.Verify(s => s.SetItemAsync<string>("authToken", "jwt"), Times.Once);
    }

    [Fact]
    public async Task LoginAsync_ReturnsFalse_OnError()
    {
        var handler = new MockHttpMessageHandler();
        handler.When(HttpMethod.Post, "http://localhost/api/auth/login")
            .Respond(HttpStatusCode.BadRequest);
        var http = handler.ToHttpClient();
        http.BaseAddress = new Uri("http://localhost");

        var storageMock = new Mock<ILocalStorageService>();
        var logger = Microsoft.Extensions.Logging.Abstractions.NullLogger<CustomAuthStateProvider>.Instance;
        var provider = new CustomAuthStateProvider(storageMock.Object, http, logger);
        var service = new AuthService(http, provider);

        var success = await service.LoginAsync(new LoginRequestDto());

        Assert.False(success);
    }
}
