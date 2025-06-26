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

    Task<List<WorkflowStepDto>> GetStepsAsync(int workflowId);
    Task<WorkflowStepDto> CreateStepAsync(WorkflowStepDto dto);
    Task<bool> UpdateStepAsync(WorkflowStepDto dto);
    Task<bool> DeleteStepAsync(int id);

    Task<List<WorkflowTransitionDto>> GetTransitionsAsync(int workflowId);
    Task<WorkflowTransitionDto> CreateTransitionAsync(WorkflowTransitionDto dto);
    Task<bool> UpdateTransitionAsync(WorkflowTransitionDto dto);
    Task<bool> DeleteTransitionAsync(int id);
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

    public async Task<List<WorkflowStepDto>> GetStepsAsync(int workflowId)
    {
        var res = await _http.GetFromJsonSafeAsync<List<WorkflowStepDto>>($"api/workflowsteps/byWorkflow/{workflowId}");
        return res ?? new();
    }

    public async Task<WorkflowStepDto> CreateStepAsync(WorkflowStepDto dto)
    {
        var res = await _http.PostAsJsonAsync("api/workflowsteps", dto);
        res.EnsureSuccessStatusCode();
        return (await res.Content.ReadFromJsonAsync<WorkflowStepDto>())!;
    }

    public async Task<bool> UpdateStepAsync(WorkflowStepDto dto)
    {
        var res = await _http.PutAsJsonAsync($"api/workflowsteps/{dto.Id}", dto);
        return res.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteStepAsync(int id)
    {
        var res = await _http.DeleteAsync($"api/workflowsteps/{id}");
        return res.IsSuccessStatusCode;
    }

    public async Task<List<WorkflowTransitionDto>> GetTransitionsAsync(int workflowId)
    {
        var res = await _http.GetFromJsonSafeAsync<List<WorkflowTransitionDto>>($"api/workflowtransitions/byWorkflow/{workflowId}");
        return res ?? new();
    }

    public async Task<WorkflowTransitionDto> CreateTransitionAsync(WorkflowTransitionDto dto)
    {
        var res = await _http.PostAsJsonAsync("api/workflowtransitions", dto);
        res.EnsureSuccessStatusCode();
        return (await res.Content.ReadFromJsonAsync<WorkflowTransitionDto>())!;
    }

    public async Task<bool> UpdateTransitionAsync(WorkflowTransitionDto dto)
    {
        var res = await _http.PutAsJsonAsync($"api/workflowtransitions/{dto.Id}", dto);
        return res.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteTransitionAsync(int id)
    {
        var res = await _http.DeleteAsync($"api/workflowtransitions/{id}");
        return res.IsSuccessStatusCode;
    }
}
