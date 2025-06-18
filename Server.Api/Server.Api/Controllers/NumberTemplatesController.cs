using GovServices.Server.Authorization;
using GovServices.Server.DTOs;
using GovServices.Server.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GovServices.Server.Controllers;

[ApiController]
[Route("api/number-templates")]
[Authorize(Roles = $"{RoleNames.Administrator},{RoleNames.Chancery},{RoleNames.ManagementHead}")]
public class NumberTemplatesController : ControllerBase
{
    private readonly INumberTemplateService _service;

    public NumberTemplatesController(INumberTemplateService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<NumberTemplateDto>>> GetAll()
    {
        return await _service.GetAllAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<NumberTemplateDto>> GetById(int id)
    {
        var item = await _service.GetByIdAsync(id);
        return item == null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<NumberTemplateDto>> Create(CreateNumberTemplateDto dto)
    {
        var result = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateNumberTemplateDto dto)
    {
        await _service.UpdateAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
