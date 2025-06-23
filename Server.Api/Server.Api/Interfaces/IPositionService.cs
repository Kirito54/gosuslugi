using GovServices.Server.DTOs;

namespace GovServices.Server.Interfaces;

public interface IPositionService
{
    Task<List<PositionDto>> GetAllAsync();
}
