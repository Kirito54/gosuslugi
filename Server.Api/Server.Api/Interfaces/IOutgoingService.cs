using GovServices.Server.DTOs;

namespace GovServices.Server.Interfaces;

public interface IOutgoingService
{
    Task<List<OutgoingDocumentDto>> GetByApplicationIdAsync(int applicationId);
    Task<OutgoingDocumentDto?> GetByIdAsync(int id);
    Task<OutgoingDocumentDto> CreateAsync(CreateOutgoingDocumentDto dto);
    Task DeleteAsync(int id);
}
