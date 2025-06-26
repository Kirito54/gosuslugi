using GovServices.Server.DTOs;

namespace GovServices.Server.Interfaces;

public interface IPermissionService
{
    Task<List<PermissionDto>> GetAllAsync();
    Task<PermissionDto> CreateAsync(CreatePermissionDto dto);
    Task UpdateAsync(int id, UpdatePermissionDto dto);
}
