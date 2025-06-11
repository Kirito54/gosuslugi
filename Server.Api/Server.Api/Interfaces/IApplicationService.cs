using GovServices.Server.DTOs;

namespace GovServices.Server.Interfaces;

public interface IApplicationService
{
    Task<List<ApplicationDto>> GetAllAsync();
    Task<ApplicationDto> GetByIdAsync(int id);
    Task<ApplicationDto> CreateAsync(CreateApplicationDto dto);
    Task UpdateAsync(int id, UpdateApplicationDto dto);
    Task DeleteAsync(int id);
    Task AdvanceAsync(int applicationId, object contextData);
    Task<List<ApplicationLogDto>> GetLogsAsync(int applicationId);
}
