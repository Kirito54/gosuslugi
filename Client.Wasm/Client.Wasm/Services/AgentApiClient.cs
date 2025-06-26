namespace Client.Wasm.Services;

using System.Net.Http.Json;
using Client.Wasm.DTOs;

public interface IAgentApiClient
{
    Task<DocumentClassificationResultDto> AnalyzeAsync(DocumentClassifyFormDto dto);
}

public class AgentApiClient(HttpClient http) : IAgentApiClient
{
    private readonly HttpClient _http = http;

    public async Task<DocumentClassificationResultDto> AnalyzeAsync(DocumentClassifyFormDto dto)
    {
        using var content = new MultipartFormDataContent();
        if (!string.IsNullOrWhiteSpace(dto.Text))
            content.Add(new StringContent(dto.Text), "Text");
        if (dto.File != null)
            content.Add(new StreamContent(dto.File.OpenReadStream()), "File", dto.File.Name);
        var res = await _http.PostAsync("api/classify", content);
        res.EnsureSuccessStatusCode();
        return (await res.Content.ReadFromJsonAsync<DocumentClassificationResultDto>())!;
    }
}
