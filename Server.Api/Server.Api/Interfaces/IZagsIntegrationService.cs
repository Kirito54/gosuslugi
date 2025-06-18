using GovServices.Server.DTOs;

namespace GovServices.Server.Interfaces;

public interface IZagsIntegrationService
{
    Task<ZagsRequestDto> SendRequestAsync(int applicationId);
    Task<ZagsRequestDto> GetStatusAsync(string requestId);
    Task<List<ZagsRequestDto>> GetByApplicationAsync(int applicationId);
}
