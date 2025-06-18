using GovServices.Server.DTOs;

namespace GovServices.Server.Interfaces;

public interface IServiceTemplateService
{
    Task<List<ServiceTemplateDto>> GetAllAsync();
    Task<ServiceTemplateDto?> GetByIdAsync(int id);
    Task<ServiceTemplateDto> CreateAsync(CreateServiceTemplateDto dto, string userId);
    Task UpdateAsync(int id, UpdateServiceTemplateDto dto, string userId);
    Task DeleteAsync(int id);
}
