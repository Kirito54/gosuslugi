using System.Net.Http.Json;
using System.Text.Json;
using GovServices.Server.Data;
using GovServices.Server.DTOs;
using GovServices.Server.Entities;
using GovServices.Server.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GovServices.Server.Services.Integrations;

public class ZagsIntegrationService : IZagsIntegrationService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    private string ApiUrl => _configuration["Zags:ApiUrl"] ?? string.Empty;

    public ZagsIntegrationService(
        IHttpClientFactory httpClientFactory,
        ApplicationDbContext context,
        IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _context = context;
        _configuration = configuration;
    }

    public async Task<ZagsRequestDto> SendRequestAsync(int applicationId)
    {
        var application = await _context.Set<Application>()
            .FirstOrDefaultAsync(a => a.Id == applicationId);
        if (application == null)
            throw new InvalidOperationException($"Application {applicationId} not found");

        var body = new { application.Id, application.Number };

        var client = _httpClientFactory.CreateClient();
        var response = await client.PostAsJsonAsync($"{ApiUrl}/request", body);
        response.EnsureSuccessStatusCode();

        var stream = await response.Content.ReadAsStreamAsync();
        var dto = await JsonSerializer.DeserializeAsync<ZagsRequestDto>(stream) ??
                  throw new InvalidOperationException("Invalid response from ZAGS");

        var entity = new ZagsRequest
        {
            ApplicationId = applicationId,
            RequestId = dto.RequestId,
            Status = dto.Status,
            ResponseXml = dto.ResponseXml,
            CreatedAt = DateTime.UtcNow
        };

        _context.Set<ZagsRequest>().Add(entity);
        await _context.SaveChangesAsync();

        dto.Id = entity.Id;
        return dto;
    }

    public async Task<ZagsRequestDto> GetStatusAsync(string requestId)
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync($"{ApiUrl}/status/{requestId}");
        response.EnsureSuccessStatusCode();

        var stream = await response.Content.ReadAsStreamAsync();
        var dto = await JsonSerializer.DeserializeAsync<ZagsRequestDto>(stream) ??
                  throw new InvalidOperationException("Invalid response from ZAGS");

        var entity = await _context.Set<ZagsRequest>()
            .FirstOrDefaultAsync(r => r.RequestId == requestId);
        if (entity != null)
        {
            entity.Status = dto.Status;
            entity.ResponseXml = dto.ResponseXml;
            await _context.SaveChangesAsync();
        }

        return dto;
    }

    public async Task<List<ZagsRequestDto>> GetByApplicationAsync(int applicationId)
    {
        var list = await _context.Set<ZagsRequest>()
            .Where(r => r.ApplicationId == applicationId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
        return list.Select(r => new ZagsRequestDto
        {
            Id = r.Id,
            ApplicationId = r.ApplicationId,
            RequestId = r.RequestId,
            Status = r.Status,
            ResponseXml = r.ResponseXml
        }).ToList();
    }
}
