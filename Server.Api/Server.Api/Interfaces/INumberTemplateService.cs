using GovServices.Server.DTOs;

namespace GovServices.Server.Interfaces;

public interface INumberTemplateService
{
    Task<List<NumberTemplateDto>> GetAllAsync();
    Task<NumberTemplateDto?> GetByIdAsync(int id);
    Task<NumberTemplateDto> CreateAsync(CreateNumberTemplateDto dto);
    Task UpdateAsync(int id, UpdateNumberTemplateDto dto);
    Task DeleteAsync(int id);
}
