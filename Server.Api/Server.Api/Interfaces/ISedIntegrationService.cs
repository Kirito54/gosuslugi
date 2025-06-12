using GovServices.Server.DTOs;

namespace GovServices.Server.Interfaces;

public interface ISedIntegrationService
{
    Task<List<SedDocumentLogDto>> GetLogsAsync(int applicationId);
    Task<bool> SendDocumentAsync(int applicationId, string documentNumber);
}
