using Microsoft.AspNetCore.Mvc;
using GovServices.Server.Interfaces;
using GovServices.Server.DTOs;

namespace GovServices.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WorkflowStepsController : ControllerBase
{
    private readonly IWorkflowService _service;

    public WorkflowStepsController(IWorkflowService service)
    {
        _service = service;
    }

    [HttpGet("byWorkflow/{workflowId}")]
    public async Task<ActionResult<List<WorkflowStepDto>>> GetByWorkflow(int workflowId)
    {
        var list = await _service.GetStepsAsync(workflowId);
        return Ok(list);
    }

    [HttpPost]
    public async Task<ActionResult<WorkflowStepDto>> Create(WorkflowStepDto dto)
    {
        var created = await _service.CreateStepAsync(dto);
        return CreatedAtAction(nameof(GetByWorkflow), new { workflowId = created.WorkflowId }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, WorkflowStepDto dto)
    {
        if (id != dto.Id) return BadRequest();
        var ok = await _service.UpdateStepAsync(dto);
        if (!ok) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _service.DeleteStepAsync(id);
        if (!ok) return NotFound();
        return NoContent();
    }
}
