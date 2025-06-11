namespace GovServices.Server.Interfaces;

using GovServices.Server.DTOs;
using GovServices.Server.Models;

public interface ITemplateService
{
    Task<List<TemplateDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TemplateDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<TemplateDto> CreateAsync(CreateTemplateDto dto, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(int id, CreateTemplateDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<byte[]> GeneratePdfAsync(int templateId, TemplateModel model, CancellationToken cancellationToken = default);
}
