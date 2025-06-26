using GovServices.Server.DTOs;

namespace GovServices.Server.Interfaces;

public interface IPositionService
{
    Task<List<PositionDto>> GetAllAsync();
    Task<PositionDto> CreateAsync(CreatePositionDto dto);
    Task UpdateAsync(int id, UpdatePositionDto dto);
}
