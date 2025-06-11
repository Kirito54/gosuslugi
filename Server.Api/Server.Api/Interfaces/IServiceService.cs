using GovServices.Server.DTOs;

namespace GovServices.Server.Interfaces;

public interface IServiceService
{
    Task<List<ServiceDto>> GetAllAsync();
    Task<ServiceDto> GetByIdAsync(int id);
    Task<ServiceDto> CreateAsync(CreateServiceDto dto);
    Task UpdateAsync(int id, UpdateServiceDto dto);
    Task DeleteAsync(int id);
}
