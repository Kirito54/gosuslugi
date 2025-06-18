using GovServices.Server.DTOs;
using GovServices.Server.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GovServices.Server.Controllers.Integrations;

[ApiController]
[Route("api/integrations/zags")] 
[Authorize]
public class ZagsController : ControllerBase
{
    private readonly IZagsIntegrationService _zags;

    public ZagsController(IZagsIntegrationService zags)
    {
        _zags = zags;
    }

    [HttpPost("request")]
    public async Task<ActionResult<ZagsRequestDto>> Create([FromBody] CreateZagsRequestDto dto)
    {
        var result = await _zags.SendRequestAsync(dto.ApplicationId);
        return Ok(result);
    }

    [HttpGet("status/{requestId}")]
    public async Task<ActionResult<ZagsRequestDto>> Status(string requestId)
    {
        var result = await _zags.GetStatusAsync(requestId);
        return Ok(result);
    }

    [HttpGet("application/{applicationId}")]
    public async Task<ActionResult<List<ZagsRequestDto>>> ByApplication(int applicationId)
    {
        var list = await _zags.GetByApplicationAsync(applicationId);
        return Ok(list);
    }
}
