using GovServices.Server.DTOs;
using GovServices.Server.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GovServices.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PageAccessesController : ControllerBase
{
    private readonly IPageAccessService _service;

    public PageAccessesController(IPageAccessService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<PageAccessDto>>> Get()
    {
        var items = await _service.GetAllAsync();
        return Ok(items);
    }

    [HttpPost]
    public async Task<ActionResult<PageAccessDto>> Post(CreatePageAccessDto dto)
    {
        var item = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
