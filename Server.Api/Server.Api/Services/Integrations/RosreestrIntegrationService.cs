using System.Net.Http.Json;
using System.Text.Json;
using GovServices.Server.Data;
using GovServices.Server.DTOs;
using GovServices.Server.Entities;
using GovServices.Server.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GovServices.Server.Services.Integrations;

public class RosreestrIntegrationService : IRosreestrIntegrationService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    private string ApiUrl => _configuration["Rosreestr:ApiUrl"] ?? string.Empty;

    public RosreestrIntegrationService(
        IHttpClientFactory httpClientFactory,
        ApplicationDbContext context,
        IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _context = context;
        _configuration = configuration;
    }

    public async Task<RosreestrRequestDto> SendRequestAsync(int applicationId)
    {
        var application = await _context.Set<Application>()
            .FirstOrDefaultAsync(a => a.Id == applicationId);
        if (application == null)
            throw new InvalidOperationException($"Application {applicationId} not found");

        var body = new { application.Id, application.Number };

        var client = _httpClientFactory.CreateClient();
        var response = await client.PostAsJsonAsync($"{ApiUrl}/sendRequest", body);
        response.EnsureSuccessStatusCode();

        var stream = await response.Content.ReadAsStreamAsync();
        var dto = await JsonSerializer.DeserializeAsync<RosreestrRequestDto>(stream) ??
                  throw new InvalidOperationException("Invalid response from Rosreestr");

        var entity = new RosreestrRequest
        {
            ApplicationId = applicationId,
            RequestId = dto.RequestId,
            Status = dto.Status,
            ResponseData = dto.ResponseData,
            CreatedAt = DateTime.UtcNow
        };

        _context.Set<RosreestrRequest>().Add(entity);
        await _context.SaveChangesAsync();

        dto.Id = entity.Id;
        return dto;
    }

    public async Task<RosreestrRequestDto> GetStatusAsync(string requestId)
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync($"{ApiUrl}/status/{requestId}");
        response.EnsureSuccessStatusCode();

        var stream = await response.Content.ReadAsStreamAsync();
        var dto = await JsonSerializer.DeserializeAsync<RosreestrRequestDto>(stream) ??
                  throw new InvalidOperationException("Invalid response from Rosreestr");

        var entity = await _context.Set<RosreestrRequest>()
            .FirstOrDefaultAsync(r => r.RequestId == requestId);
        if (entity != null)
        {
            entity.Status = dto.Status;
            entity.ResponseData = dto.ResponseData;
            await _context.SaveChangesAsync();
        }

        return dto;
    }
}

