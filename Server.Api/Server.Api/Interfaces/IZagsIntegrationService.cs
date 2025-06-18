using GovServices.Server.DTOs;

namespace GovServices.Server.Interfaces;

public interface IZagsIntegrationService
{
    Task<ZagsRequestDto> SendRequestAsync(CreateZagsRequestDto dto);
    Task<ZagsRequestDto> GetStatusAsync(string requestId);
}
