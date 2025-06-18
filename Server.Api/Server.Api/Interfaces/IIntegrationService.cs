using System.Threading.Tasks;

namespace GovServices.Server.Interfaces;

public interface IIntegrationService<TCreateDto, TResponseDto>
{
    Task<TResponseDto> SendRequestAsync(TCreateDto dto);
    Task<TResponseDto> GetStatusAsync(string requestId);
}
