using GovServices.Server.DTOs;

namespace GovServices.Server.Interfaces;

public interface IPermissionGroupService
{
    Task<List<PermissionGroupDto>> GetAllAsync();
    Task<PermissionGroupDto> CreateAsync(CreatePermissionGroupDto dto);
    Task UpdateAsync(int id, UpdatePermissionGroupDto dto);
}
