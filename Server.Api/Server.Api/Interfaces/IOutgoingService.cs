using GovServices.Server.DTOs;

namespace GovServices.Server.Interfaces;

public interface IOutgoingService
{
    Task<List<OutgoingDocumentDto>> GetByApplicationIdAsync(int applicationId);
    Task<OutgoingDocumentDto?> GetByIdAsync(int id);
    Task<OutgoingDocumentDto> CreateAsync(CreateOutgoingDocumentDto dto);
    Task<bool> DeleteAsync(int id);
}
