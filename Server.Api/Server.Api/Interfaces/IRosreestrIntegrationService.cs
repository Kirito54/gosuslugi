using GovServices.Server.DTOs;

namespace GovServices.Server.Interfaces;

public interface IRosreestrIntegrationService
{
    Task<RosreestrRequestDto> SendRequestAsync(int applicationId);
    Task<RosreestrRequestDto> GetStatusAsync(string requestId);
    Task<List<RosreestrRequestDto>> GetByApplicationAsync(int applicationId);
}
