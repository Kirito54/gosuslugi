using GovServices.Server.DTOs;
using GovServices.Server.Models;

namespace GovServices.Server.Interfaces;

public interface ITemplateService
{
    Task<List<TemplateDto>> GetAllAsync();
    Task<TemplateDto?> GetByIdAsync(int id);
    Task<TemplateDto> CreateAsync(CreateTemplateDto dto);
    Task UpdateAsync(int id, UpdateTemplateDto dto);
    Task DeleteAsync(int id);
    Task<byte[]> GeneratePdfAsync(int templateId, TemplateModel model);
}
