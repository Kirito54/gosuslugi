namespace Client.Wasm.Services;

using System.Net.Http.Json;
using Client.Wasm.DTOs;
using Microsoft.AspNetCore.Components.Forms;
using Client.Wasm.Helpers;

public interface IDocumentApiClient
{
    Task<List<DocumentDto>> GetByOwnerAsync(Guid ownerId);
    Task<DocumentDto?> GetByIdAsync(Guid id);
    Task<string> GetBase64Async(Guid id);
    Task<Guid> UploadAsync(DocumentUploadDto dto);
    Task UploadSignatureAsync(DocumentSignatureDto dto);
    Task DeleteAsync(Guid id);
}

public class DocumentApiClient : IDocumentApiClient
{
    private readonly HttpClient _http;

    public DocumentApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<DocumentDto>> GetByOwnerAsync(Guid ownerId)
    {
        var result = await _http.GetFromJsonSafeAsync<List<DocumentDto>>($"api/documents/owner/{ownerId}");
        return result ?? new();
    }

    public async Task<DocumentDto?> GetByIdAsync(Guid id)
    {
        return await _http.GetFromJsonSafeAsync<DocumentDto>($"api/documents/{id}");
    }

    public async Task<string> GetBase64Async(Guid id)
    {
        return await _http.GetStringAsync($"api/documents/base64/{id}");
    }

    public async Task<Guid> UploadAsync(DocumentUploadDto dto)
    {
        using var content = new MultipartFormDataContent();
        content.Add(new StreamContent(dto.File.OpenReadStream()), "File", dto.File.Name);
        content.Add(new StringContent(dto.OwnerId.ToString()), "OwnerId");
        content.Add(new StringContent(dto.Type.ToString()), "Type");
        content.Add(new StringContent(dto.Visibility.ToString()), "Visibility");
        var res = await _http.PostAsync("api/documents/upload", content);
        res.EnsureSuccessStatusCode();
        var idStr = await res.Content.ReadAsStringAsync();
        return Guid.Parse(idStr.Trim('"'));
    }

    public async Task UploadSignatureAsync(DocumentSignatureDto dto)
    {
        var res = await _http.PostAsJsonAsync("api/documents/signature", dto);
        res.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(Guid id)
    {
        var res = await _http.DeleteAsync($"api/documents/{id}");
        res.EnsureSuccessStatusCode();
    }
}
