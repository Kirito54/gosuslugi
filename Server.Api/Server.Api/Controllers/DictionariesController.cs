using GovServices.Server.DTOs;
using GovServices.Server.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GovServices.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Администратор,Системный аналитик")]
public class DictionariesController : ControllerBase
{
    private readonly IDictionaryService _service;

    public DictionariesController(IDictionaryService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<DictionaryDto>>> GetAll()
    {
        var list = await _service.GetAllAsync();
        return Ok(list);
    }

    [HttpPost]
    [DisableRequestSizeLimit]
    public async Task<ActionResult<int>> Upload([FromForm] UploadDictionaryDto dto)
    {
        var id = await _service.CreateAsync(dto);
        return Ok(id);
    }
}
