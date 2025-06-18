namespace Client.Wasm.Services;

using System.Net.Http.Json;
using Client.Wasm.DTOs;

public interface IAgentApiClient
{
    Task<AgentResponseDto> AnalyzeAsync(AgentRequestDto dto);
}

public class AgentApiClient(HttpClient http) : IAgentApiClient
{
    private readonly HttpClient _http = http;

    public async Task<AgentResponseDto> AnalyzeAsync(AgentRequestDto dto)
    {
        var res = await _http.PostAsJsonAsync("analyze", dto);
        res.EnsureSuccessStatusCode();
        return (await res.Content.ReadFromJsonAsync<AgentResponseDto>())!;
    }
}
