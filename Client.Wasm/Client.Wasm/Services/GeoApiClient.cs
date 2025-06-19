namespace Client.Wasm.Services;

using System.Net.Http.Json;
using Client.Wasm.DTOs;
using Client.Wasm.Helpers;

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
        var result = await _http.GetFromJsonSafeAsync<List<GeoObjectDto>>("api/geoobjects");
        return result ?? new();
    }

    public async Task<GeoObjectDto?> GetByIdAsync(int id)
    {
        return await _http.GetFromJsonSafeAsync<GeoObjectDto>($"api/geoobjects/{id}");
    }

    public async Task<GeoObjectDto> CreateAsync(CreateGeoObjectDto dto)
    {
        var res = await _http.PostAsJsonAsync("api/geoobjects", dto);
        res.EnsureSuccessStatusCode();
        return (await res.Content.ReadFromJsonAsync<GeoObjectDto>())!;
    }

    public async Task<List<GeoObjectDto>> GetIntersectingAsync(string wkt)
    {
        var result = await _http.GetFromJsonSafeAsync<List<GeoObjectDto>>($"api/geoobjects/intersect?wkt={Uri.EscapeDataString(wkt)}");
        return result ?? new();
    }

    public async Task DeleteAsync(int id)
    {
        var res = await _http.DeleteAsync($"api/geoobjects/{id}");
        res.EnsureSuccessStatusCode();
    }
}
