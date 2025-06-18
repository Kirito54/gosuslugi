using Microsoft.AspNetCore.Mvc;
using GovServices.Server.Interfaces;
using GovServices.Server.DTOs;

namespace GovServices.Server.Controllers;

[ApiController]
[Route("api/document-templates")]
public class DocumentTemplatesController : ControllerBase
{
    private readonly ITemplateService _templates;

    public DocumentTemplatesController(ITemplateService templates)
    {
        _templates = templates;
    }

    [HttpGet]
    public async Task<ActionResult<List<TemplateDto>>> GetAll()
    {
        var list = await _templates.GetAllAsync();
        return list.Where(t => t.Type == "Word").ToList();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TemplateDto>> GetById(int id)
    {
        var item = await _templates.GetByIdAsync(id);
        if (item == null || item.Type != "Word") return NotFound();
        return item;
    }

    [HttpPost]
    public async Task<ActionResult<TemplateDto>> Create(CreateTemplateDto dto)
    {
        dto.Type = "Word";
        var result = await _templates.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateTemplateDto dto)
    {
        dto.Type = "Word";
        await _templates.UpdateAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _templates.DeleteAsync(id);
        return NoContent();
    }
}
