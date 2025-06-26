using AutoMapper;
using GovServices.Server.Data;
using GovServices.Server.DTOs;
using GovServices.Server.Entities;
using GovServices.Server.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace GovServices.Server.Services;

public class WorkflowService : IWorkflowService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public WorkflowService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<WorkflowDto>> GetAllWorkflowsAsync(CancellationToken cancellationToken = default)
    {
        var workflows = await _context.Set<Workflow>()
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        return _mapper.Map<List<WorkflowDto>>(workflows);
    }

    public async Task<WorkflowDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var workflow = await _context.Set<Workflow>()
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.Id == id, cancellationToken);
        return workflow == null ? null : _mapper.Map<WorkflowDto>(workflow);
    }

    public async Task<WorkflowDto> CreateAsync(WorkflowDto dto, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<Workflow>(dto);
        _context.Set<Workflow>().Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return _mapper.Map<WorkflowDto>(entity);
    }

    public async Task<bool> UpdateAsync(WorkflowDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _context.Set<Workflow>().FirstOrDefaultAsync(w => w.Id == dto.Id, cancellationToken);
        if (entity == null)
            return false;

        entity.Name = dto.Name;
        entity.Description = dto.Description;
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.Set<Workflow>().FirstOrDefaultAsync(w => w.Id == id, cancellationToken);
        if (entity == null)
            return false;

        _context.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> CanTransitionAsync(int fromStepId, int toStepId, object? contextData)
    {
        var transition = await _context.Set<WorkflowTransition>()
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.FromStepId == fromStepId && t.ToStepId == toStepId);
        if (transition == null)
            return false;

        if (string.IsNullOrWhiteSpace(transition.ConditionExpression))
            return true;

        var options = ScriptOptions.Default
            .AddReferences(typeof(object).Assembly)
            .AddImports("System");

        try
        {
            return await CSharpScript.EvaluateAsync<bool>(transition.ConditionExpression, options, globals: contextData);
        }
        catch
        {
            return false;
        }
    }

    public async Task<WorkflowStepDto?> GetNextStepAsync(int currentStepId, object? contextData)
    {
        var transitions = await _context.Set<WorkflowTransition>()
            .Where(t => t.FromStepId == currentStepId)
            .ToListAsync();

        foreach (var t in transitions)
        {
            if (await CanTransitionAsync(currentStepId, t.ToStepId, contextData))
            {
                var step = await _context.Set<WorkflowStep>().FindAsync(t.ToStepId);
                return step == null ? null : _mapper.Map<WorkflowStepDto>(step);
            }
        }
        return null;
    }
}
