using GovServices.Server.DTOs;

namespace GovServices.Server.Interfaces;

public interface IDepartmentService
{
    Task<List<DepartmentDto>> GetAllAsync();
}
