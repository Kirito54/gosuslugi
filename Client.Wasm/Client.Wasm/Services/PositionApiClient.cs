namespace Client.Wasm.Services;

using System.Net.Http.Json;
using Client.Wasm.DTOs;
using Client.Wasm.Helpers;

public interface IPositionApiClient
{
    Task<List<PositionDto>> GetAllAsync();
    Task<PositionDto> CreateAsync(CreatePositionDto dto);
    Task UpdateAsync(int id, UpdatePositionDto dto);
}

public class PositionApiClient : IPositionApiClient
{
    private readonly HttpClient _http;

    public PositionApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<PositionDto>> GetAllAsync()
    {
        var result = await _http.GetFromJsonSafeAsync<List<PositionDto>>("api/positions");
        return result ?? new();
    }

    public async Task<PositionDto> CreateAsync(CreatePositionDto dto)
    {
        var response = await _http.PostAsJsonAsync("api/positions", dto);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<PositionDto>())!;
    }

    public async Task UpdateAsync(int id, UpdatePositionDto dto)
    {
        var response = await _http.PutAsJsonAsync($"api/positions/{id}", dto);
        response.EnsureSuccessStatusCode();
    }
}
