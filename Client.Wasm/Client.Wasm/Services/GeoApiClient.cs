namespace Client.Wasm.Services;

using System.Net.Http.Json;
using Client.Wasm.DTOs;

public interface IGeoApiClient
{
    Task<List<GeoObjectDto>> GetAllAsync();
    Task<GeoObjectDto?> GetByIdAsync(int id);
    Task<GeoObjectDto> CreateAsync(CreateGeoObjectDto dto);
    Task<List<GeoObjectDto>> GetIntersectingAsync(string wkt);
    Task DeleteAsync(int id);
}

public class GeoApiClient : IGeoApiClient
{
    private readonly HttpClient _http;

    public GeoApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<GeoObjectDto>> GetAllAsync()
    {
        return await _http.GetFromJsonAsync<List<GeoObjectDto>>("api/geoobjects") ?? new();
    }

    public async Task<GeoObjectDto?> GetByIdAsync(int id)
    {
        return await _http.GetFromJsonAsync<GeoObjectDto>($"api/geoobjects/{id}");
    }

    public async Task<GeoObjectDto> CreateAsync(CreateGeoObjectDto dto)
    {
        var res = await _http.PostAsJsonAsync("api/geoobjects", dto);
        res.EnsureSuccessStatusCode();
        return (await res.Content.ReadFromJsonAsync<GeoObjectDto>())!;
    }

    public async Task<List<GeoObjectDto>> GetIntersectingAsync(string wkt)
    {
        return await _http.GetFromJsonAsync<List<GeoObjectDto>>($"api/geoobjects/intersect?wkt={Uri.EscapeDataString(wkt)}") ?? new();
    }

    public async Task DeleteAsync(int id)
    {
        var res = await _http.DeleteAsync($"api/geoobjects/{id}");
        res.EnsureSuccessStatusCode();
    }
}
