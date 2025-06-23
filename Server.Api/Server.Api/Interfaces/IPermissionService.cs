using GovServices.Server.DTOs;

namespace GovServices.Server.Interfaces;

public interface IPermissionService
{
    Task<List<PermissionDto>> GetAllAsync();
}
