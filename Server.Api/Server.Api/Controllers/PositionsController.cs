using Microsoft.AspNetCore.Mvc;
using GovServices.Server.Interfaces;
using GovServices.Server.DTOs;

namespace GovServices.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PositionsController : ControllerBase
{
    private readonly IPositionService _service;
    public PositionsController(IPositionService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<PositionDto>>> Get()
    {
        var items = await _service.GetAllAsync();
        return Ok(items);
    }

    [HttpPost]
    public async Task<ActionResult<PositionDto>> Create(CreatePositionDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(Get), new { }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdatePositionDto dto)
    {
        await _service.UpdateAsync(id, dto);
        return NoContent();
    }
}
