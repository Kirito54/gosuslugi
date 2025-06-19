namespace Client.Wasm.Services;

using System.Net.Http.Json;
using Client.Wasm.DTOs;
using Client.Wasm.Helpers;

public interface IWorkflowApiClient
{
    Task<List<WorkflowDto>> GetAllWorkflowsAsync();
    Task<WorkflowDto?> GetByIdAsync(int id);
    Task<WorkflowDto> CreateAsync(WorkflowDto dto);
    Task<bool> UpdateAsync(WorkflowDto dto);
    Task<bool> DeleteAsync(int id);
    Task<bool> CanTransitionAsync(int fromStepId, int toStepId, object? contextData);
    Task<WorkflowStepDto?> GetNextStepAsync(int currentStepId, object? contextData);
}

public class WorkflowApiClient : IWorkflowApiClient
{
    private readonly HttpClient _http;

    public WorkflowApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<WorkflowDto>> GetAllWorkflowsAsync()
    {
        var result = await _http.GetFromJsonSafeAsync<List<WorkflowDto>>("api/workflow");
        return result ?? new();
    }

    public async Task<WorkflowDto?> GetByIdAsync(int id)
    {
        return await _http.GetFromJsonSafeAsync<WorkflowDto>($"api/workflow/{id}");
    }

    public async Task<WorkflowDto> CreateAsync(WorkflowDto dto)
    {
        var res = await _http.PostAsJsonAsync("api/workflow", dto);
        res.EnsureSuccessStatusCode();
        return (await res.Content.ReadFromJsonAsync<WorkflowDto>())!;
    }

    public async Task<bool> UpdateAsync(WorkflowDto dto)
    {
        var res = await _http.PutAsJsonAsync($"api/workflow/{dto.Id}", dto);
        return res.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var res = await _http.DeleteAsync($"api/workflow/{id}");
        return res.IsSuccessStatusCode;
    }

    public async Task<bool> CanTransitionAsync(int fromStepId, int toStepId, object? contextData)
    {
        var res = await _http.PostAsJsonAsync($"api/workflow/canTransition?from={fromStepId}&to={toStepId}", contextData);
        return res.IsSuccessStatusCode && await res.Content.ReadFromJsonAsync<bool>();
    }

    public async Task<WorkflowStepDto?> GetNextStepAsync(int currentStepId, object? contextData)
    {
        var res = await _http.PostAsJsonAsync($"api/workflow/nextStep/{currentStepId}", contextData);
        if (!res.IsSuccessStatusCode) return null;
        return await res.Content.ReadFromJsonAsync<WorkflowStepDto>();
    }
}
