using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using GovServices.Server.Controllers;
using GovServices.Server.Interfaces;
using GovServices.Server.DTOs;

namespace Server.Tests;

public class AuthControllerTests
{
    [Fact]
    public async Task Login_ReturnsToken_WhenCredentialsValid()
    {
        var dto = new LoginRequestDto { Email = "admin@test.com", Password = "pass" };
        var authMock = new Mock<IAuthService>();
        authMock.Setup(a => a.LoginAsync(dto)).ReturnsAsync(new AuthResultDto
        {
            Token = "token",
            RefreshToken = "ref",
            Expiration = DateTime.UtcNow.AddHours(1)
        });
        var controller = new AuthController(authMock.Object);

        var result = await controller.Login(dto);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Login_ReturnsUnauthorized_WhenInvalid()
    {
        var dto = new LoginRequestDto { Email = "bad@test.com", Password = "bad" };
        var authMock = new Mock<IAuthService>();
        authMock.Setup(a => a.LoginAsync(dto)).ThrowsAsync(new UnauthorizedAccessException());
        var controller = new AuthController(authMock.Object);

        var result = await controller.Login(dto);

        Assert.IsType<UnauthorizedObjectResult>(result);
    }
}
