using Microsoft.AspNetCore.Mvc;
using GovServices.Server.Interfaces;
using GovServices.Server.DTOs;

namespace GovServices.Server.Controllers;

[ApiController]
[Route("api/applications/{applicationId}/results")]
public class ApplicationResultsController : ControllerBase
{
    private readonly IApplicationService _apps;

    public ApplicationResultsController(IApplicationService apps)
    {
        _apps = apps;
    }

    [HttpGet]
    public async Task<ActionResult<List<ApplicationResultDto>>> GetAll(int applicationId)
    {
        return await _apps.GetResultsAsync(applicationId);
    }

    [HttpPost]
    public async Task<ActionResult<ApplicationResultDto>> Create(int applicationId, CreateApplicationResultDto dto)
    {
        if (applicationId != dto.ApplicationId) return BadRequest();
        return await _apps.AddResultAsync(dto);
    }
}
