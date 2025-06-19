namespace Client.Wasm.Services;

using System.Net.Http.Json;
using Client.Wasm.DTOs;
using Client.Wasm.Helpers;

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
        var result = await _http.GetFromJsonSafeAsync<List<PermissionGroupDto>>("api/permissiongroups");
        return result ?? new();
    }
}
