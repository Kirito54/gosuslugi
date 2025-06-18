using GovServices.Server.Data;
using GovServices.Server.Entities;
using GovServices.Server.Services.Integrations;
using GovServices.Server.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net;

public class RosreestrIntegrationServiceTests
{
    [Fact]
    public async Task SendRequestAsync_SavesEntity()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("rosreestr_send")
            .Options;
        var context = new ApplicationDbContext(options);
        context.Applications.Add(new Application { Id = 1, Number = "0001", Status = "New", AssignedToUserId = "u" });
        context.SaveChanges();

        var handler = new DelegateHandler(_ =>
        {
            var dto = new RosreestrRequestDto { RequestId = "r1", Status = "В процессе", ResponseData = "<xml/>" };
            var json = System.Text.Json.JsonSerializer.Serialize(dto);
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
            };
        });
        var client = new HttpClient(handler);
        var factory = new FakeHttpClientFactory(client);
        var config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string,string>{{"Rosreestr:ApiUrl","http://test"}}).Build();
        var service = new RosreestrIntegrationService(factory, context, config);

        var result = await service.SendRequestAsync(1);

        Assert.Equal("r1", result.RequestId);
        Assert.Equal(1, await context.RosreestrRequests.CountAsync());
    }
}
