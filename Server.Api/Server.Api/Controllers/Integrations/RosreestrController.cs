using GovServices.Server.DTOs;
using GovServices.Server.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GovServices.Server.Controllers.Integrations;

[ApiController]
[Route("api/integrations/rosreestr")]
[Authorize]
public class RosreestrController : ControllerBase
{
    private readonly IRosreestrIntegrationService _rosreestr;

    public RosreestrController(IRosreestrIntegrationService rosreestr)
    {
        _rosreestr = rosreestr;
    }

    [HttpPost("request")]
    public async Task<ActionResult<RosreestrRequestDto>> Create([FromBody] CreateRosreestrRequestDto dto)
    {
        var result = await _rosreestr.SendRequestAsync(dto.ApplicationId);
        return Ok(result);
    }

    [HttpGet("status/{requestId}")]
    public async Task<ActionResult<RosreestrRequestDto>> Status(string requestId)
    {
        var result = await _rosreestr.GetStatusAsync(requestId);
        return Ok(result);
    }

    [HttpGet("application/{applicationId}")]
    public async Task<ActionResult<List<RosreestrRequestDto>>> ByApplication(int applicationId)
    {
        var list = await _rosreestr.GetByApplicationAsync(applicationId);
        return Ok(list);
    }
}
