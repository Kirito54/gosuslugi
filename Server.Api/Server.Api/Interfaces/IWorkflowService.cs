namespace GovServices.Server.Interfaces;

using GovServices.Server.DTOs;

public interface IWorkflowService
{
    Task<List<WorkflowDto>> GetAllWorkflowsAsync(CancellationToken cancellationToken = default);
    Task<WorkflowDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<WorkflowDto> CreateAsync(WorkflowDto dto, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(WorkflowDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> CanTransitionAsync(int fromStepId, int toStepId, object? contextData);
    Task<WorkflowStepDto?> GetNextStepAsync(int currentStepId, object? contextData);
}
