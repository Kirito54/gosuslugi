using Microsoft.AspNetCore.Mvc;
using GovServices.Server.Interfaces;
using GovServices.Server.DTOs;

namespace GovServices.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WorkflowTransitionsController : ControllerBase
{
    private readonly IWorkflowService _service;

    public WorkflowTransitionsController(IWorkflowService service)
    {
        _service = service;
    }

    [HttpGet("byWorkflow/{workflowId}")]
    public async Task<ActionResult<List<WorkflowTransitionDto>>> GetByWorkflow(int workflowId)
    {
        var list = await _service.GetTransitionsAsync(workflowId);
        return Ok(list);
    }

    [HttpPost]
    public async Task<ActionResult<WorkflowTransitionDto>> Create(WorkflowTransitionDto dto)
    {
        var created = await _service.CreateTransitionAsync(dto);
        return CreatedAtAction(nameof(GetByWorkflow), new { workflowId = created.FromStepId }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, WorkflowTransitionDto dto)
    {
        if (id != dto.Id) return BadRequest();
        var ok = await _service.UpdateTransitionAsync(dto);
        if (!ok) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _service.DeleteTransitionAsync(id);
        if (!ok) return NotFound();
        return NoContent();
    }
}
