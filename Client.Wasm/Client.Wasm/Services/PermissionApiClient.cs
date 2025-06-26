namespace Client.Wasm.Services;

using System.Net.Http.Json;
using Client.Wasm.DTOs;
using Client.Wasm.Helpers;

public interface IPermissionApiClient
{
    Task<List<PermissionDto>> GetAllAsync();
    Task<PermissionDto> CreateAsync(CreatePermissionDto dto);
    Task UpdateAsync(int id, UpdatePermissionDto dto);
}

public class PermissionApiClient : IPermissionApiClient
{
    private readonly HttpClient _http;

    public PermissionApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<PermissionDto>> GetAllAsync()
    {
        var result = await _http.GetFromJsonSafeAsync<List<PermissionDto>>("api/permissions");
        return result ?? new();
    }

    public async Task<PermissionDto> CreateAsync(CreatePermissionDto dto)
    {
        var response = await _http.PostAsJsonAsync("api/permissions", dto);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<PermissionDto>())!;
    }

    public async Task UpdateAsync(int id, UpdatePermissionDto dto)
    {
        var res = await _http.PutAsJsonAsync($"api/permissions/{id}", dto);
        res.EnsureSuccessStatusCode();
    }
}
