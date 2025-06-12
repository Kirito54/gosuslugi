using GovServices.Server.Data;
using GovServices.Server.DTOs;
using GovServices.Server.Entities;
using GovServices.Server.Interfaces;

namespace GovServices.Server.Services;

public class AuditService : IAuditService
{
    private readonly ApplicationDbContext _context;

    public AuditService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task LogAsync(AuditLogDto dto)
    {
        var ent = new AuditLog
        {
            UserName = dto.UserName,
            ActionType = dto.ActionType,
            EntityType = dto.EntityType,
            EntityId = dto.EntityId,
            Timestamp = dto.Timestamp,
            DurationMs = dto.DurationMs
        };

        _context.AuditLogs.Add(ent);
        await _context.SaveChangesAsync();
    }
}
