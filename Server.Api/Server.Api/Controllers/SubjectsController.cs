using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GovServices.Server.Interfaces;
using GovServices.Server.DTOs;

namespace GovServices.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubjectsController : ControllerBase
{
    private readonly ISubjectService _service;

    public SubjectsController(ISubjectService service)
    {
        _service = service;
    }

    [Authorize]
    [HttpGet("individuals")]
    public async Task<ActionResult<List<IndividualDto>>> GetIndividuals()
    {
        var items = await _service.GetIndividualsAsync();
        return Ok(items);
    }

    [Authorize]
    [HttpGet("legal-entities")]
    public async Task<ActionResult<List<LegalEntityDto>>> GetLegalEntities()
    {
        var items = await _service.GetLegalEntitiesAsync();
        return Ok(items);
    }
}
