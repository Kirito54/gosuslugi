using Microsoft.AspNetCore.Mvc;
using GovServices.Server.Interfaces;
using GovServices.Server.DTOs;

namespace GovServices.Server.Controllers;

[ApiController]
[Route("api/applications/{applicationId}/revisions")]
public class ApplicationRevisionsController : ControllerBase
{
    private readonly IApplicationService _apps;

    public ApplicationRevisionsController(IApplicationService apps)
    {
        _apps = apps;
    }

    [HttpGet]
    public async Task<ActionResult<List<ApplicationRevisionDto>>> GetAll(int applicationId)
    {
        return await _apps.GetRevisionsAsync(applicationId);
    }

    [HttpPost]
    public async Task<ActionResult<ApplicationRevisionDto>> Create(int applicationId, CreateApplicationRevisionDto dto)
    {
        if (applicationId != dto.ApplicationId) return BadRequest();
        return await _apps.AddRevisionAsync(dto);
    }
}
