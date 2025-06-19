namespace Client.Wasm.Services;

using System.Net.Http.Json;
using Client.Wasm.DTOs;
using Client.Wasm.Helpers;

public interface IOutgoingApiClient
{
    Task<List<OutgoingDocumentDto>> GetByApplicationIdAsync(int applicationId);
    Task<OutgoingDocumentDto?> GetByIdAsync(int id);
    Task<OutgoingDocumentDto> CreateAsync(int applicationId, Stream fileStream, string fileName, IEnumerable<(Stream stream, string fileName)> attachments);
    Task DeleteAsync(int id);
}

public class OutgoingApiClient : IOutgoingApiClient
{
    private readonly HttpClient _http;

    public OutgoingApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<OutgoingDocumentDto>> GetByApplicationIdAsync(int applicationId)
    {
        var result = await _http.GetFromJsonSafeAsync<List<OutgoingDocumentDto>>($"api/applications/{applicationId}/outgoing");
        return result ?? new();
    }

    public async Task<OutgoingDocumentDto?> GetByIdAsync(int id)
    {
        return await _http.GetFromJsonSafeAsync<OutgoingDocumentDto>($"api/outgoing/{id}");
    }

    public async Task<OutgoingDocumentDto> CreateAsync(int applicationId, Stream fileStream, string fileName, IEnumerable<(Stream stream, string fileName)> attachments)
    {
        using var content = new MultipartFormDataContent();
        content.Add(new StreamContent(fileStream), "file", fileName);
        int i = 0;
        foreach (var att in attachments)
        {
            content.Add(new StreamContent(att.stream), $"attachments[{i}]", att.fileName);
            i++;
        }
        var res = await _http.PostAsync("api/outgoing", content);
        res.EnsureSuccessStatusCode();
        return (await res.Content.ReadFromJsonAsync<OutgoingDocumentDto>())!;
    }

    public async Task DeleteAsync(int id)
    {
        var res = await _http.DeleteAsync($"api/outgoing/{id}");
        res.EnsureSuccessStatusCode();
    }
}
