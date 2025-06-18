using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using GovServices.Server.Controllers;
using GovServices.Server.Interfaces;
using GovServices.Server.DTOs;

namespace Server.Tests;

public class ZayavlenieControllerTests
{
    [Fact]
    public async Task GetByApplicant_ReturnsOk()
    {
        var serviceMock = new Mock<IApplicationService>();
        serviceMock.Setup(s => s.GetByApplicantAsync(1)).ReturnsAsync(new List<ApplicationDto>());
        var controller = new ApplicationsController(serviceMock.Object);

        var result = await controller.GetRelatedByApplicant(1);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.IsType<List<ApplicationDto>>(ok.Value);
    }

    [Fact]
    public async Task GetByRepresentative_ReturnsOk()
    {
        var serviceMock = new Mock<IApplicationService>();
        serviceMock.Setup(s => s.GetByRepresentativeAsync(2)).ReturnsAsync(new List<ApplicationDto>());
        var controller = new ApplicationsController(serviceMock.Object);

        var result = await controller.GetRelatedByRepresentative(2);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        Assert.IsType<List<ApplicationDto>>(ok.Value);
    }

    [Fact]
    public async Task AttachResult_ReturnsCreatedResult()
    {
        var dto = new CreateApplicationResultDto { ApplicationId = 5 };
        var serviceMock = new Mock<IApplicationService>();
        serviceMock.Setup(s => s.AddResultAsync(dto)).ReturnsAsync(new ApplicationResultDto { ApplicationId = 5 });
        var controller = new ApplicationResultsController(serviceMock.Object);

        var result = await controller.Create(5, dto);

        var ok = Assert.IsType<ApplicationResultDto>(result.Value);
        Assert.Equal(5, ok.ApplicationId);
    }
}
