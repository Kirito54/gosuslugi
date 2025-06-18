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

public class ZagsIntegrationService : IntegrationServiceBase<CreateZagsRequestDto, ZagsRequestDto>, IZagsIntegrationService
{
    private readonly ILogger<ZagsIntegrationService> _logger;
    private readonly IConfiguration _configuration;

    protected override string ServiceName => "ZAGS";
    protected override string ApiUrl => _configuration["Zags:ApiUrl"] ?? string.Empty;
    protected override string Login => _configuration["Zags:Login"] ?? string.Empty;
    protected override string Password => _configuration["Zags:Password"] ?? string.Empty;

    public ZagsIntegrationService(
        IHttpClientFactory httpClientFactory,
        ApplicationDbContext context,
        IConfiguration configuration,
        ILogger<ZagsIntegrationService> logger)
        : base(httpClientFactory, context, configuration, logger)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public override async Task<ZagsRequestDto> SendRequestAsync(CreateZagsRequestDto dto)
    {
        var result = await SendWithAuthAsync<ZagsRequestDto, CreateZagsRequestDto>($"{ApiUrl}/api/Zags/InformApi/CreateInformRequst", HttpMethod.Post, dto);

        var entity = new ZagsRequest
        {
            ApplicationId = dto.ApplicationId,
            RequestId = result.RequestId,
            Status = result.Status,
            ResponseXml = result.ResponseXml,
            CreatedAt = DateTime.UtcNow
        };
        Context.Set<ZagsRequest>().Add(entity);
        await Context.SaveChangesAsync();
        result.Id = entity.Id;
        return result;
    }

    public override async Task<ZagsRequestDto> GetStatusAsync(string requestId)
    {
        var apiDto = await SendWithAuthAsync<ZagsStatusApiDto, object?>($"{ApiUrl}/api/Zags/StatusApi/GetStatus/{requestId}", HttpMethod.Get, null);

        var dto = new ZagsRequestDto
        {
            RequestId = requestId,
            Status = apiDto.StatusCode,
            ResponseXml = apiDto.Data.ResponceXml,
            Attachments = apiDto.Data.Attachments
        };

        var entity = await Context.Set<ZagsRequest>()
            .Include(r => r.Attachments)
            .FirstOrDefaultAsync(r => r.RequestId == requestId);
        if (entity != null)
        {
            entity.Status = dto.Status;
            entity.ResponseXml = dto.ResponseXml;
            entity.Attachments.Clear();
            foreach (var att in dto.Attachments ?? new())
            {
                entity.Attachments.Add(new ZagsRequestAttachment
                {
                    FileName = att.FileName,
                    Content = Convert.FromBase64String(att.ContentBase64)
                });
            }
            await Context.SaveChangesAsync();
        }
        return dto;
    }
}
