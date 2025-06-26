using Microsoft.AspNetCore.Mvc;
using GovServices.Server.Interfaces;
using GovServices.Server.DTOs;

namespace GovServices.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServicesController : ControllerBase
{
    private readonly IServiceService _services;

    public ServicesController(IServiceService services)
    {
        _services = services;
    }

    [HttpGet]
    public async Task<ActionResult<List<ServiceDto>>> GetAll()
    {
        var result = await _services.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ServiceDto>> GetById(int id)
    {
        var item = await _services.GetByIdAsync(id);
        return Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<ServiceDto>> Create(CreateServiceDto dto)
    {
        var result = await _services.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateServiceDto dto)
    {
        await _services.UpdateAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _services.DeleteAsync(id);
        return NoContent();
    }
}
