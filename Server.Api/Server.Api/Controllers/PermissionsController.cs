using Microsoft.AspNetCore.Mvc;
using GovServices.Server.Interfaces;
using GovServices.Server.DTOs;

namespace GovServices.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PermissionsController : ControllerBase
{
    private readonly IPermissionService _service;
    public PermissionsController(IPermissionService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<PermissionDto>>> Get()
    {
        var items = await _service.GetAllAsync();
        return Ok(items);
    }

    [HttpPost]
    public async Task<ActionResult<PermissionDto>> Create(CreatePermissionDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(Get), new { }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdatePermissionDto dto)
    {
        await _service.UpdateAsync(id, dto);
        return NoContent();
    }
}
