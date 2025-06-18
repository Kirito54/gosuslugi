using GovServices.Server.DTOs;

namespace GovServices.Server.Interfaces;

public interface IDocumentStorageService
{
    Task<Guid> SaveAsync(DocumentUploadDto dto);
    Task<Stream> GetFileStreamAsync(Guid id);
    Task<bool> DeleteAsync(Guid id);
}
