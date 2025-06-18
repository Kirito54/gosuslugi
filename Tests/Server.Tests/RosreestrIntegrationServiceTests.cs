using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using GovServices.Server.Data;
using GovServices.Server.Entities;
using GovServices.Server.Services.Integrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

public class RosreestrIntegrationServiceTests
{
    [Fact]
    public async Task SendRequestAsync_ApplicationNotFound_Throws()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("testdb1").Options;
        var context = new ApplicationDbContext(options);
        var factory = new Mock<IHttpClientFactory>();
        var config = new ConfigurationBuilder().AddInMemoryCollection().Build();
        var service = new RosreestrIntegrationService(factory.Object, context, config);
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.SendRequestAsync(1));
    }
}
