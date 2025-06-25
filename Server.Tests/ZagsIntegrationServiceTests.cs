using GovServices.Server.Data;
using GovServices.Server.Entities;
using GovServices.Server.Services.Integrations;
using GovServices.Server.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net;

public class ZagsIntegrationServiceTests
{
    [Fact]
    public async Task SendRequestAsync_SavesEntity()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("zags_send")
            .Options;
        var context = new ApplicationDbContext(options);
        context.Applications.Add(new Application { Id = 1, Number = "0001", Status = "New", AssignedToUserId = "u" });
        context.SaveChanges();

        var handler = new DelegateHandler(_ =>
        {
            var dto = new ZagsRequestDto { RequestId = "z1", Status = "В процессе", ResponseXml = "<xml/>" };
            var json = System.Text.Json.JsonSerializer.Serialize(dto);
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
            };
        });
        var client = new HttpClient(handler);
        var factory = new FakeHttpClientFactory(client);
        var config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string>
        {
            {"Zags:ApiUrl","http://test"},
            {"Zags:Login","login"},
            {"Zags:Password","pass"}
        }).Build();
        var logger = Microsoft.Extensions.Logging.Abstractions.NullLogger<ZagsIntegrationService>.Instance;
        var service = new ZagsIntegrationService(factory, context, config, logger);

        var dtoResult = await service.SendRequestAsync(new CreateZagsRequestDto { ApplicationId = 1 });

        Assert.Equal("z1", dtoResult.RequestId);
        Assert.Equal(1, await context.Set<ZagsRequest>().CountAsync());
    }
}
