using GovServices.Server.DTOs;

namespace GovServices.Server.Interfaces;

public interface IDictionaryService
{
    Task<List<DictionaryDto>> GetAllAsync();
    Task<DictionaryDto?> GetByIdAsync(int id);
    Task<int> CreateAsync(UploadDictionaryDto dto);
}
