namespace Client.Wasm.Services;

using System.Net.Http.Json;
using Client.Wasm.DTOs;
using Client.Wasm.Helpers;

public interface IPermissionApiClient
{
    Task<List<PermissionDto>> GetAllAsync();
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
}
