namespace Client.Wasm.Services;

using System.Net.Http.Json;
using Client.Wasm.DTOs;
using Client.Wasm.Helpers;

public interface IServiceApiClient
{
    Task<List<ServiceDto>> GetAllAsync();
    Task<ServiceDto?> GetByIdAsync(int id);
    Task<ServiceDto> CreateAsync(CreateServiceDto dto);
    Task UpdateAsync(int id, UpdateServiceDto dto);
    Task DeleteAsync(int id);
}

public class ServiceApiClient : IServiceApiClient
{
    private readonly HttpClient _http;

    public ServiceApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<ServiceDto>> GetAllAsync()
    {
        var result = await _http.GetFromJsonSafeAsync<List<ServiceDto>>("api/services");
        return result ?? new();
    }

    public async Task<ServiceDto?> GetByIdAsync(int id)
    {
        return await _http.GetFromJsonSafeAsync<ServiceDto>($"api/services/{id}");
    }

    public async Task<ServiceDto> CreateAsync(CreateServiceDto dto)
    {
        var response = await _http.PostAsJsonAsync("api/services", dto);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<ServiceDto>())!;
    }

    public async Task UpdateAsync(int id, UpdateServiceDto dto)
    {
        var response = await _http.PutAsJsonAsync($"api/services/{id}", dto);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(int id)
    {
        var response = await _http.DeleteAsync($"api/services/{id}");
        response.EnsureSuccessStatusCode();
    }
}
