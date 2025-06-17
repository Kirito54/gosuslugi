namespace Client.Wasm.Services;

using System.Net.Http.Json;
using Client.Wasm.DTOs;

public interface IPermissionGroupApiClient
{
    Task<List<PermissionGroupDto>> GetAllAsync();
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
        return await _http.GetFromJsonAsync<List<PermissionGroupDto>>("api/permissiongroups") ?? new();
    }
}
