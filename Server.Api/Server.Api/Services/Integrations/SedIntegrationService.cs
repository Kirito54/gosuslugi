using GovServices.Server.Data;
using GovServices.Server.DTOs;
using GovServices.Server.Entities;
using GovServices.Server.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GovServices.Server.Services.Integrations;

public class SedIntegrationService : ISedIntegrationService
{
    private readonly ApplicationDbContext _context;

    public SedIntegrationService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<SedDocumentLogDto>> GetLogsAsync(int applicationId)
    {
        var logs = await _context.Set<SedDocumentLog>()
            .Where(l => l.ApplicationId == applicationId)
            .ToListAsync();

        return logs.Select(l => new SedDocumentLogDto
        {
            Id = l.Id,
            ApplicationId = l.ApplicationId,
            DocumentNumber = l.DocumentNumber,
            Action = l.Action,
            Timestamp = l.Timestamp
        }).ToList();
    }

    public async Task<bool> SendDocumentAsync(int applicationId, string documentNumber)
    {
        _context.Set<SedDocumentLog>().Add(new SedDocumentLog
        {
            ApplicationId = applicationId,
            DocumentNumber = documentNumber,
            Action = "Sent",
            Timestamp = DateTime.UtcNow
        });

        await _context.SaveChangesAsync();
        return true;
    }
}

