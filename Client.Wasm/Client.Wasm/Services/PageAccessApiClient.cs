namespace Client.Wasm.Services;

using System.Net.Http.Json;
using Client.Wasm.DTOs;
using Client.Wasm.Helpers;

public interface IPageAccessApiClient
{
    Task<List<PageAccessDto>> GetAllAsync();
    Task<PageAccessDto> CreateAsync(CreatePageAccessDto dto);
    Task DeleteAsync(int id);
}

public class PageAccessApiClient : IPageAccessApiClient
{
    private readonly HttpClient _http;

    public PageAccessApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<PageAccessDto>> GetAllAsync()
    {
        var result = await _http.GetFromJsonSafeAsync<List<PageAccessDto>>("api/pageaccesses");
        return result ?? new();
    }

    public async Task<PageAccessDto> CreateAsync(CreatePageAccessDto dto)
    {
        var res = await _http.PostAsJsonAsync("api/pageaccesses", dto);
        res.EnsureSuccessStatusCode();
        return (await res.Content.ReadFromJsonAsync<PageAccessDto>())!;
    }

    public async Task DeleteAsync(int id)
    {
        var res = await _http.DeleteAsync($"api/pageaccesses/{id}");
        res.EnsureSuccessStatusCode();
    }
}
