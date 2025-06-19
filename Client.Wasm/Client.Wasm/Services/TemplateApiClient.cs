namespace Client.Wasm.Services;

using System.Net.Http.Json;
using Client.Wasm.DTOs;
using Client.Wasm.Helpers;

public interface ITemplateApiClient
{
    Task<List<TemplateDto>> GetAllAsync();
    Task<TemplateDto?> GetByIdAsync(int id);
    Task<TemplateDto> CreateAsync(CreateTemplateDto dto);
    Task UpdateAsync(int id, UpdateTemplateDto dto);
    Task DeleteAsync(int id);
    Task<byte[]> GeneratePdfAsync(int templateId, TemplateModel model);
}

public class TemplateApiClient : ITemplateApiClient
{
    private readonly HttpClient _http;

    public TemplateApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<TemplateDto>> GetAllAsync()
    {
        var result = await _http.GetFromJsonSafeAsync<List<TemplateDto>>("api/templates");
        return result ?? new();
    }

    public async Task<TemplateDto?> GetByIdAsync(int id)
    {
        return await _http.GetFromJsonSafeAsync<TemplateDto>($"api/templates/{id}");
    }

    public async Task<TemplateDto> CreateAsync(CreateTemplateDto dto)
    {
        var res = await _http.PostAsJsonAsync("api/templates", dto);
        res.EnsureSuccessStatusCode();
        return (await res.Content.ReadFromJsonAsync<TemplateDto>())!;
    }

    public async Task UpdateAsync(int id, UpdateTemplateDto dto)
    {
        var res = await _http.PutAsJsonAsync($"api/templates/{id}", dto);
        res.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(int id)
    {
        var res = await _http.DeleteAsync($"api/templates/{id}");
        res.EnsureSuccessStatusCode();
    }

    public async Task<byte[]> GeneratePdfAsync(int templateId, TemplateModel model)
    {
        var res = await _http.PostAsJsonAsync($"api/templates/{templateId}/generate", model);
        res.EnsureSuccessStatusCode();
        return (await res.Content.ReadFromJsonAsync<byte[]>())!;
    }
}
