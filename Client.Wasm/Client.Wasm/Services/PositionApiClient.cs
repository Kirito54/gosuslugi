namespace Client.Wasm.Services;

using System.Net.Http.Json;
using Client.Wasm.DTOs;
using Client.Wasm.Helpers;

public interface IPositionApiClient
{
    Task<List<PositionDto>> GetAllAsync();
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
}
