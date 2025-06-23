namespace Client.Wasm.Services;

using System.Net.Http.Json;
using Client.Wasm.DTOs;
using Client.Wasm.Helpers;

public interface IDepartmentApiClient
{
    Task<List<DepartmentDto>> GetAllAsync();
}

public class DepartmentApiClient : IDepartmentApiClient
{
    private readonly HttpClient _http;

    public DepartmentApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<DepartmentDto>> GetAllAsync()
    {
        var result = await _http.GetFromJsonSafeAsync<List<DepartmentDto>>("api/departments");
        return result ?? new();
    }
}
