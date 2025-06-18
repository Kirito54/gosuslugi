namespace Client.Wasm.Services;

using System.Net.Http.Json;
using Client.Wasm.DTOs;

public interface IServiceTemplateApiClient
{
    Task<List<ServiceTemplateDto>> GetAllAsync();
    Task<ServiceTemplateDto?> GetByIdAsync(int id);
    Task<ServiceTemplateDto> CreateAsync(CreateServiceTemplateDto dto);
    Task UpdateAsync(int id, UpdateServiceTemplateDto dto);
    Task DeleteAsync(int id);
}

public class ServiceTemplateApiClient : IServiceTemplateApiClient
{
    private readonly HttpClient _http;

    public ServiceTemplateApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<ServiceTemplateDto>> GetAllAsync()
    {
        return await _http.GetFromJsonAsync<List<ServiceTemplateDto>>("api/servicetemplates") ?? new();
    }

    public async Task<ServiceTemplateDto?> GetByIdAsync(int id)
    {
        return await _http.GetFromJsonAsync<ServiceTemplateDto>($"api/servicetemplates/{id}");
    }

    public async Task<ServiceTemplateDto> CreateAsync(CreateServiceTemplateDto dto)
    {
        var res = await _http.PostAsJsonAsync("api/servicetemplates", dto);
        res.EnsureSuccessStatusCode();
        return (await res.Content.ReadFromJsonAsync<ServiceTemplateDto>())!;
    }

    public async Task UpdateAsync(int id, UpdateServiceTemplateDto dto)
    {
        var res = await _http.PutAsJsonAsync($"api/servicetemplates/{id}", dto);
        res.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(int id)
    {
        var res = await _http.DeleteAsync($"api/servicetemplates/{id}");
        res.EnsureSuccessStatusCode();
    }
}
