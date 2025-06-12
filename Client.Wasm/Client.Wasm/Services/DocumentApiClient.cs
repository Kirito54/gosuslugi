namespace Client.Wasm.Services;

using System.Net.Http.Json;
using Client.Wasm.DTOs;

public interface IDocumentApiClient
{
    Task<List<DocumentDto>> GetByApplicationIdAsync(int applicationId);
    Task<DocumentDto?> GetByIdAsync(int id);
    Task<DocumentDto> UploadAsync(int applicationId, Stream fileStream, string fileName);
    Task DeleteAsync(int id);
}

public class DocumentApiClient : IDocumentApiClient
{
    private readonly HttpClient _http;

    public DocumentApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<DocumentDto>> GetByApplicationIdAsync(int applicationId)
    {
        return await _http.GetFromJsonAsync<List<DocumentDto>>($"api/applications/{applicationId}/documents") ?? new();
    }

    public async Task<DocumentDto?> GetByIdAsync(int id)
    {
        return await _http.GetFromJsonAsync<DocumentDto>($"api/documents/{id}");
    }

    public async Task<DocumentDto> UploadAsync(int applicationId, Stream fileStream, string fileName)
    {
        using var content = new MultipartFormDataContent();
        content.Add(new StreamContent(fileStream), "file", fileName);
        var res = await _http.PostAsync($"api/applications/{applicationId}/documents", content);
        res.EnsureSuccessStatusCode();
        return (await res.Content.ReadFromJsonAsync<DocumentDto>())!;
    }

    public async Task DeleteAsync(int id)
    {
        var res = await _http.DeleteAsync($"api/documents/{id}");
        res.EnsureSuccessStatusCode();
    }
}
