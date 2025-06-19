namespace Client.Wasm.Services;

using System.Net.Http.Json;
using Client.Wasm.DTOs;
using Client.Wasm.Helpers;

public interface IApplicationApiClient
{
    Task<List<ApplicationDto>> GetAllAsync();
    Task<ApplicationDto?> GetByIdAsync(int id);
    Task<ApplicationDto> CreateAsync(CreateApplicationDto dto);
    Task UpdateAsync(int id, UpdateApplicationDto dto);
    Task DeleteAsync(int id);
    Task AdvanceAsync(int applicationId, object contextData);
    Task<List<ApplicationLogDto>> GetLogsAsync(int applicationId);
    Task<List<ApplicationResultDto>> GetResultsAsync(int applicationId);
    Task<ApplicationResultDto> AddResultAsync(CreateApplicationResultDto dto);
    Task<List<ApplicationRevisionDto>> GetRevisionsAsync(int applicationId);
    Task<ApplicationRevisionDto> AddRevisionAsync(CreateApplicationRevisionDto dto);

    Task<List<ApplicationDto>> GetRelatedByApplicantAsync(int applicationId);
    Task<List<ApplicationDto>> GetRelatedByRepresentativeAsync(int applicationId);
    Task<List<ApplicationDto>> GetRelatedByGeoAsync(int applicationId);
}

public class ApplicationApiClient : IApplicationApiClient
{
    private readonly HttpClient _http;

    public ApplicationApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<ApplicationDto>> GetAllAsync()
    {
        var result = await _http.GetFromJsonSafeAsync<List<ApplicationDto>>("api/applications");
        return result ?? new();
    }

    public async Task<ApplicationDto?> GetByIdAsync(int id)
    {
        return await _http.GetFromJsonSafeAsync<ApplicationDto>($"api/applications/{id}");
    }

    public async Task<ApplicationDto> CreateAsync(CreateApplicationDto dto)
    {
        var res = await _http.PostAsJsonAsync("api/applications", dto);
        res.EnsureSuccessStatusCode();
        return (await res.Content.ReadFromJsonAsync<ApplicationDto>())!;
    }

    public async Task UpdateAsync(int id, UpdateApplicationDto dto)
    {
        var res = await _http.PutAsJsonAsync($"api/applications/{id}", dto);
        res.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(int id)
    {
        var res = await _http.DeleteAsync($"api/applications/{id}");
        res.EnsureSuccessStatusCode();
    }

    public async Task AdvanceAsync(int applicationId, object contextData)
    {
        var res = await _http.PostAsJsonAsync($"api/applications/{applicationId}/advance", contextData);
        res.EnsureSuccessStatusCode();
    }

    public async Task<List<ApplicationLogDto>> GetLogsAsync(int applicationId)
    {
        var result = await _http.GetFromJsonSafeAsync<List<ApplicationLogDto>>($"api/applications/{applicationId}/logs");
        return result ?? new();
    }

    public async Task<List<ApplicationResultDto>> GetResultsAsync(int applicationId)
    {
        var result = await _http.GetFromJsonSafeAsync<List<ApplicationResultDto>>($"api/applications/{applicationId}/results");
        return result ?? new();
    }

    public async Task<ApplicationResultDto> AddResultAsync(CreateApplicationResultDto dto)
    {
        var res = await _http.PostAsJsonAsync($"api/applications/{dto.ApplicationId}/results", dto);
        res.EnsureSuccessStatusCode();
        return (await res.Content.ReadFromJsonAsync<ApplicationResultDto>())!;
    }

    public async Task<List<ApplicationRevisionDto>> GetRevisionsAsync(int applicationId)
    {
        var result = await _http.GetFromJsonSafeAsync<List<ApplicationRevisionDto>>($"api/applications/{applicationId}/revisions");
        return result ?? new();
    }

    public async Task<ApplicationRevisionDto> AddRevisionAsync(CreateApplicationRevisionDto dto)
    {
        var res = await _http.PostAsJsonAsync($"api/applications/{dto.ApplicationId}/revisions", dto);
        res.EnsureSuccessStatusCode();
        return (await res.Content.ReadFromJsonAsync<ApplicationRevisionDto>())!;
    }

    public async Task<List<ApplicationDto>> GetRelatedByApplicantAsync(int applicationId)
    {
        var result = await _http.GetFromJsonSafeAsync<List<ApplicationDto>>($"api/applications/{applicationId}/related/applicant");
        return result ?? new();
    }

    public async Task<List<ApplicationDto>> GetRelatedByRepresentativeAsync(int applicationId)
    {
        var result = await _http.GetFromJsonSafeAsync<List<ApplicationDto>>($"api/applications/{applicationId}/related/representative");
        return result ?? new();
    }

    public async Task<List<ApplicationDto>> GetRelatedByGeoAsync(int applicationId)
    {
        var result = await _http.GetFromJsonSafeAsync<List<ApplicationDto>>($"api/applications/{applicationId}/related/geo");
        return result ?? new();
    }
}
