using GovServices.Server.DTOs;
using Microsoft.AspNetCore.Http;

namespace GovServices.Server.Interfaces;

public interface IDocumentService
{
    Task<List<DocumentDto>> GetByApplicationIdAsync(int applicationId);
    Task<DocumentDto?> GetByIdAsync(int id);
    Task<DocumentDto> UploadAsync(int applicationId, IFormFile file);
    Task DeleteAsync(int id);
}
