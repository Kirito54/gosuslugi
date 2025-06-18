using System.Net.Http.Json;
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

    public ZagsIntegrationService(IHttpClientFactory httpClientFactory,
        ApplicationDbContext context,
        IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _context = context;
        _configuration = configuration;
    }

    public async Task<ZagsRequestDto> SendRequestAsync(CreateZagsRequestDto dto)
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.PostAsJsonAsync($"{ApiUrl}/request", dto);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<ZagsRequestDto>() ??
                     throw new InvalidOperationException("Invalid response from ZAGS");

        var entity = new ZagsRequest
        {
            ApplicationId = dto.ApplicationId,
            RequestId = result.RequestId,
            Status = result.Status,
            ResponseXml = result.ResponseXml,
            CreatedAt = DateTime.UtcNow
        };

        _context.Set<ZagsRequest>().Add(entity);
        await _context.SaveChangesAsync();

        result.Id = entity.Id;
        return result;
    }

    public async Task<ZagsRequestDto> GetStatusAsync(string requestId)
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync($"{ApiUrl}/status/{requestId}");
        response.EnsureSuccessStatusCode();

        var dto = await response.Content.ReadFromJsonAsync<ZagsRequestDto>() ??
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
}
