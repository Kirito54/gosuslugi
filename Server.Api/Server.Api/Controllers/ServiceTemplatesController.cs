using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using GovServices.Server.Entities;
using GovServices.Server.Interfaces;
using GovServices.Server.DTOs;
using GovServices.Server.Authorization;

namespace GovServices.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServiceTemplatesController : ControllerBase
{
    private readonly IServiceTemplateService _service;
    private readonly UserManager<ApplicationUser> _userManager;

    public ServiceTemplatesController(IServiceTemplateService service, UserManager<ApplicationUser> userManager)
    {
        _service = service;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<ActionResult<List<ServiceTemplateDto>>> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ServiceTemplateDto?>> GetById(int id)
    {
        var res = await _service.GetByIdAsync(id);
        return res == null ? NotFound() : Ok(res);
    }

    [Authorize(Roles = RoleNames.Administrator)]
    [HttpPost]
    public async Task<ActionResult<ServiceTemplateDto>> Create(CreateServiceTemplateDto dto)
    {
        var userId = _userManager.GetUserId(User) ?? string.Empty;
        var res = await _service.CreateAsync(dto, userId);
        return Ok(res);
    }

    [Authorize(Roles = RoleNames.Administrator)]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateServiceTemplateDto dto)
    {
        var userId = _userManager.GetUserId(User) ?? string.Empty;
        await _service.UpdateAsync(id, dto, userId);
        return NoContent();
    }

    [Authorize(Roles = RoleNames.Administrator)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
