namespace Client.Wasm.Services;

using System.Net.Http.Json;
using Client.Wasm.DTOs;
using Client.Wasm.Helpers;

public interface IIntegrationApiClient
{
    Task<RosreestrRequestDto> SendRosreestrRequestAsync(int applicationId);
    Task<RosreestrRequestDto> GetRosreestrStatusAsync(string requestId);
    Task<ZagsRequestDto> SendZagsRequestAsync(CreateZagsRequestDto dto);
    Task<ZagsRequestDto> GetZagsStatusAsync(string requestId);
    Task<List<SedDocumentLogDto>> GetSedLogsAsync(int applicationId);
    Task<bool> SendSedDocumentAsync(int applicationId, string documentNumber);
    Task<byte[]> SignPdfAsync(byte[] pdfBytes, string certificateThumbprint);
    Task<bool> VerifySignatureAsync(byte[] signedPdfBytes);
}

public class IntegrationApiClient : IIntegrationApiClient
{
    private readonly HttpClient _http;

    public IntegrationApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<RosreestrRequestDto> SendRosreestrRequestAsync(int applicationId)
    {
        var res = await _http.PostAsync($"api/integrations/rosreestr/{applicationId}", null);
        res.EnsureSuccessStatusCode();
        return (await res.Content.ReadFromJsonAsync<RosreestrRequestDto>())!;
    }

    public async Task<RosreestrRequestDto> GetRosreestrStatusAsync(string requestId)
    {
        var result = await _http.GetFromJsonSafeAsync<RosreestrRequestDto>($"api/integrations/rosreestr/status/{requestId}");
        return result ?? new RosreestrRequestDto();
    }

    public async Task<ZagsRequestDto> SendZagsRequestAsync(CreateZagsRequestDto dto)
    {
        var res = await _http.PostAsJsonAsync("api/integrations/zags/request", dto);
        res.EnsureSuccessStatusCode();
        return (await res.Content.ReadFromJsonAsync<ZagsRequestDto>())!;
    }

    public async Task<ZagsRequestDto> GetZagsStatusAsync(string requestId)
    {
        var result = await _http.GetFromJsonSafeAsync<ZagsRequestDto>($"api/integrations/zags/status/{requestId}");
        return result ?? new ZagsRequestDto();
    }

    public async Task<List<SedDocumentLogDto>> GetSedLogsAsync(int applicationId)
    {
        var result = await _http.GetFromJsonSafeAsync<List<SedDocumentLogDto>>($"api/integrations/sed/{applicationId}/logs");
        return result ?? new();
    }

    public async Task<bool> SendSedDocumentAsync(int applicationId, string documentNumber)
    {
        var res = await _http.PostAsJsonAsync($"api/integrations/sed/{applicationId}", new { documentNumber });
        if (!res.IsSuccessStatusCode) return false;
        return await res.Content.ReadFromJsonAsync<bool>();
    }

    public async Task<byte[]> SignPdfAsync(byte[] pdfBytes, string certificateThumbprint)
    {
        var res = await _http.PostAsJsonAsync("api/integrations/ecp/sign", new { pdfBytes, certificateThumbprint });
        res.EnsureSuccessStatusCode();
        return (await res.Content.ReadFromJsonAsync<byte[]>())!;
    }

    public async Task<bool> VerifySignatureAsync(byte[] signedPdfBytes)
    {
        var res = await _http.PostAsJsonAsync("api/integrations/ecp/verify", signedPdfBytes);
        if (!res.IsSuccessStatusCode) return false;
        return await res.Content.ReadFromJsonAsync<bool>();
    }
}
