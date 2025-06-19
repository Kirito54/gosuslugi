namespace Client.Wasm.Services;

using System.Net.Http.Json;
using Client.Wasm.DTOs;
using Microsoft.AspNetCore.Components.Forms;
using Client.Wasm.Helpers;

public interface IDictionaryApiClient
{
    Task<List<DictionaryDto>> GetAllAsync();
    Task<int> UploadAsync(UploadDictionaryDto dto);
}

public class DictionaryApiClient : IDictionaryApiClient
{
    private readonly HttpClient _http;

    public DictionaryApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<DictionaryDto>> GetAllAsync()
    {
        var result = await _http.GetFromJsonSafeAsync<List<DictionaryDto>>("api/dictionaries");
        return result ?? new();
    }

    public async Task<int> UploadAsync(UploadDictionaryDto dto)
    {
        using var content = new MultipartFormDataContent();
        content.Add(new StringContent(dto.Name), "Name");
        content.Add(new StringContent(dto.Description), "Description");
        content.Add(new StringContent(dto.SourceType), "SourceType");
        if (!string.IsNullOrWhiteSpace(dto.SourceUrl))
            content.Add(new StringContent(dto.SourceUrl), "SourceUrl");
        content.Add(new StreamContent(dto.File.OpenReadStream()), "File", dto.File.Name);
        var res = await _http.PostAsync("api/dictionaries", content);
        res.EnsureSuccessStatusCode();
        var str = await res.Content.ReadAsStringAsync();
        return int.Parse(str.Trim('"'));
    }
}
