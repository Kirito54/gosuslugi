using Microsoft.AspNetCore.Mvc;
using GovServices.Server.Interfaces;
using GovServices.Server.DTOs;

namespace GovServices.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PermissionGroupsController : ControllerBase
{
    private readonly IPermissionGroupService _service;

    public PermissionGroupsController(IPermissionGroupService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IEnumerable<PermissionGroupDto>> Get()
    {
        return await _service.GetAllAsync();
    }

    [HttpPost]
    public async Task<ActionResult<PermissionGroupDto>> Create(CreatePermissionGroupDto dto)
    {
        var created = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(Get), new { }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdatePermissionGroupDto dto)
    {
        await _service.UpdateAsync(id, dto);
        return NoContent();
    }
}
