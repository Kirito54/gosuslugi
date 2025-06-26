namespace Client.Wasm.Services;

using System.Net.Http.Json;
using Client.Wasm.DTOs;
using Client.Wasm.Helpers;

public interface IPermissionGroupApiClient
{
    Task<List<PermissionGroupDto>> GetAllAsync();
    Task<PermissionGroupDto> CreateAsync(CreatePermissionGroupDto dto);
    Task UpdateAsync(int id, UpdatePermissionGroupDto dto);
}

public class PermissionGroupApiClient : IPermissionGroupApiClient
{
    private readonly HttpClient _http;

    public PermissionGroupApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<PermissionGroupDto>> GetAllAsync()
    {
        var result = await _http.GetFromJsonSafeAsync<List<PermissionGroupDto>>("api/permissiongroups");
        return result ?? new();
    }

    public async Task<PermissionGroupDto> CreateAsync(CreatePermissionGroupDto dto)
    {
        var res = await _http.PostAsJsonAsync("api/permissiongroups", dto);
        res.EnsureSuccessStatusCode();
        return (await res.Content.ReadFromJsonAsync<PermissionGroupDto>())!;
    }

    public async Task UpdateAsync(int id, UpdatePermissionGroupDto dto)
    {
        var res = await _http.PutAsJsonAsync($"api/permissiongroups/{id}", dto);
        res.EnsureSuccessStatusCode();
    }
}
