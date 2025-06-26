using GovServices.Server.DTOs;

namespace GovServices.Server.Interfaces;

public interface IPageAccessService
{
    Task<List<PageAccessDto>> GetAllAsync();
    Task<PageAccessDto> CreateAsync(CreatePageAccessDto dto);
    Task DeleteAsync(int id);
}
