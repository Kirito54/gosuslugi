using Microsoft.AspNetCore.Mvc;
using GovServices.Server.Interfaces;
using GovServices.Server.DTOs;

namespace GovServices.Server.Controllers.Integrations;

[ApiController]
[Route("api/integrations/zags")]
public class ZagsController : ControllerBase
{
    private readonly IZagsIntegrationService _zags;

    public ZagsController(IZagsIntegrationService zags)
    {
        _zags = zags;
    }

    [HttpPost("request")]
    public async Task<IActionResult> Create([FromBody] CreateZagsRequestDto dto)
    {
        var result = await _zags.SendRequestAsync(dto);
        return Ok(result);
    }

    [HttpGet("status/{requestId}")]
    public async Task<IActionResult> Status(string requestId)
    {
        var result = await _zags.GetStatusAsync(requestId);
        return Ok(result);
    }
}
