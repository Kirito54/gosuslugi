namespace Client.Wasm.Services;

using System.Net.Http.Json;
using Client.Wasm.DTOs;
using Client.Wasm.Helpers;

public interface IDepartmentApiClient
{
    Task<List<DepartmentDto>> GetAllAsync();
    Task<DepartmentDto> CreateAsync(CreateDepartmentDto dto);
    Task UpdateAsync(int id, UpdateDepartmentDto dto);
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

    public async Task<DepartmentDto> CreateAsync(CreateDepartmentDto dto)
    {
        var response = await _http.PostAsJsonAsync("api/departments", dto);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<DepartmentDto>())!;
    }

    public async Task UpdateAsync(int id, UpdateDepartmentDto dto)
    {
        var response = await _http.PutAsJsonAsync($"api/departments/{id}", dto);
        response.EnsureSuccessStatusCode();
    }
}
