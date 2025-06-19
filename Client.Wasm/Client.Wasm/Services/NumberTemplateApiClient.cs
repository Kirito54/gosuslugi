namespace Client.Wasm.Services;

using System.Net.Http.Json;
using Client.Wasm.DTOs;
using Client.Wasm.Helpers;

public interface INumberTemplateApiClient
{
    Task<List<NumberTemplateDto>> GetAllAsync();
    Task<NumberTemplateDto?> GetByIdAsync(int id);
    Task<NumberTemplateDto> CreateAsync(CreateNumberTemplateDto dto);
    Task UpdateAsync(int id, UpdateNumberTemplateDto dto);
    Task DeleteAsync(int id);
}

public class NumberTemplateApiClient : INumberTemplateApiClient
{
    private readonly HttpClient _http;
    public NumberTemplateApiClient(HttpClient http) => _http = http;

    public async Task<List<NumberTemplateDto>> GetAllAsync()
    {
        var result = await _http.GetFromJsonSafeAsync<List<NumberTemplateDto>>("api/number-templates");
        return result ?? new();
    }

    public async Task<NumberTemplateDto?> GetByIdAsync(int id)
        => await _http.GetFromJsonSafeAsync<NumberTemplateDto>($"api/number-templates/{id}");

    public async Task<NumberTemplateDto> CreateAsync(CreateNumberTemplateDto dto)
    {
        var res = await _http.PostAsJsonAsync("api/number-templates", dto);
        res.EnsureSuccessStatusCode();
        return (await res.Content.ReadFromJsonAsync<NumberTemplateDto>())!;
    }

    public async Task UpdateAsync(int id, UpdateNumberTemplateDto dto)
    {
        var res = await _http.PutAsJsonAsync($"api/number-templates/{id}", dto);
        res.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(int id)
    {
        var res = await _http.DeleteAsync($"api/number-templates/{id}");
        res.EnsureSuccessStatusCode();
    }
}
