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

    Task<List<WorkflowStepDto>> GetStepsAsync(int workflowId, CancellationToken cancellationToken = default);
    Task<WorkflowStepDto> CreateStepAsync(WorkflowStepDto dto, CancellationToken cancellationToken = default);
    Task<bool> UpdateStepAsync(WorkflowStepDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteStepAsync(int id, CancellationToken cancellationToken = default);

    Task<List<WorkflowTransitionDto>> GetTransitionsAsync(int workflowId, CancellationToken cancellationToken = default);
    Task<WorkflowTransitionDto> CreateTransitionAsync(WorkflowTransitionDto dto, CancellationToken cancellationToken = default);
    Task<bool> UpdateTransitionAsync(WorkflowTransitionDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteTransitionAsync(int id, CancellationToken cancellationToken = default);
}
