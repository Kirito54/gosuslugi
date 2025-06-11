using GovServices.Server.DTOs;

namespace GovServices.Server.Interfaces;

public interface IAuditService
{
    Task LogAsync(AuditLogDto dto);
}
