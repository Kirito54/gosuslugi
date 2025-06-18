using System;
using System.Net.Http.Json;
using GovServices.Server.Data;
using GovServices.Server.DTOs;
using GovServices.Server.Entities;
using GovServices.Server.Interfaces;
using GovServices.Server.Services.Integrations.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GovServices.Server.Services.Integrations;

public class RosreestrIntegrationService : IntegrationServiceBase<CreateRosreestrRequestDto, RosreestrRequestDto>, IRosreestrIntegrationService
{
    private readonly ILogger<RosreestrIntegrationService> _logger;
    private readonly IConfiguration _configuration;

    protected override string ServiceName => "Rosreestr";
    protected override string ApiUrl => _configuration["Rosreestr:ApiUrl"] ?? string.Empty;
    protected override string Login => _configuration["Rosreestr:Login"] ?? string.Empty;
    protected override string Password => _configuration["Rosreestr:Password"] ?? string.Empty;

    public RosreestrIntegrationService(
        IHttpClientFactory httpClientFactory,
        ApplicationDbContext context,
        IConfiguration configuration,
        ILogger<RosreestrIntegrationService> logger)
        : base(httpClientFactory, context, configuration, logger)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public override async Task<RosreestrRequestDto> SendRequestAsync(CreateRosreestrRequestDto dto)
    {
        var app = await Context.Applications.FirstOrDefaultAsync(a => a.Id == dto.ApplicationId)
            ?? throw new InvalidOperationException($"Application {dto.ApplicationId} not found");

        var body = new { app.Id, app.Number };
        var result = await SendWithAuthAsync<RosreestrRequestDto, object>($"{ApiUrl}/api/request", HttpMethod.Post, body);

        var entity = new RosreestrRequest
        {
            ApplicationId = dto.ApplicationId,
            RequestId = result.RequestId,
            Status = result.Status,
            ResponseData = result.ResponseData,
            CreatedAt = DateTime.UtcNow
        };
        Context.Set<RosreestrRequest>().Add(entity);
        await Context.SaveChangesAsync();
        result.Id = entity.Id;
        return result;
    }

    public override async Task<RosreestrRequestDto> GetStatusAsync(string requestId)
    {
        var dto = await SendWithAuthAsync<RosreestrRequestDto, object?>($"{ApiUrl}/api/status/{requestId}", HttpMethod.Get, null);

        var entity = await Context.Set<RosreestrRequest>()
            .Include(r => r.Attachments)
            .FirstOrDefaultAsync(r => r.RequestId == requestId);
        if (entity != null)
        {
            entity.Status = dto.Status;
            entity.ResponseData = dto.ResponseData;
            await Context.SaveChangesAsync();
        }

        return dto;
    }
}
