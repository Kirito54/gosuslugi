using Microsoft.AspNetCore.Mvc;
using GovServices.Server.Interfaces;

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

    [HttpPost("advance")]
    public IActionResult Advance()
    {
        return Ok();
    }
}
