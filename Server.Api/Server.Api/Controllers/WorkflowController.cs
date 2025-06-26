using Microsoft.AspNetCore.Mvc;
using GovServices.Server.Interfaces;
using GovServices.Server.DTOs;

namespace GovServices.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WorkflowController : ControllerBase
{
    private readonly IWorkflowService _workflows;

    public WorkflowController(IWorkflowService workflows)
    {
        _workflows = workflows;
    }

    [HttpGet]
    public async Task<ActionResult<List<WorkflowDto>>> GetAll()
    {
        var items = await _workflows.GetAllWorkflowsAsync();
        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<WorkflowDto?>> GetById(int id)
    {
        var item = await _workflows.GetByIdAsync(id);
        if (item == null) return NotFound();
        return Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<WorkflowDto>> Create(WorkflowDto dto)
    {
        var item = await _workflows.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, WorkflowDto dto)
    {
        if (id != dto.Id) return BadRequest();
        var success = await _workflows.UpdateAsync(dto);
        if (!success) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _workflows.DeleteAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }

    [HttpPost("canTransition")]
    public async Task<ActionResult<bool>> CanTransition(int from, int to, [FromBody] object? contextData)
    {
        var result = await _workflows.CanTransitionAsync(from, to, contextData);
        return Ok(result);
    }

    [HttpPost("nextStep/{currentStepId}")]
    public async Task<ActionResult<WorkflowStepDto?>> GetNextStep(int currentStepId, [FromBody] object? contextData)
    {
        var next = await _workflows.GetNextStepAsync(currentStepId, contextData);
        if (next == null) return NotFound();
        return Ok(next);
    }
}
