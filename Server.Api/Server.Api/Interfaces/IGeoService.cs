using GovServices.Server.DTOs;

namespace GovServices.Server.Interfaces;

public interface IGeoService
{
    Task<List<GeoObjectDto>> GetAllAsync();
    Task<GeoObjectDto?> GetByIdAsync(int id);
    Task<GeoObjectDto> CreateAsync(CreateGeoObjectDto dto);
    Task<List<GeoObjectDto>> GetIntersectingAsync(string wkt);
    Task DeleteAsync(int id);
}
