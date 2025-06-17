using Microsoft.AspNetCore.Mvc;
using GovServices.Server.Interfaces;
using GovServices.Server.DTOs;

namespace GovServices.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApplicationsController : ControllerBase
{
    private readonly IApplicationService _applications;

    public ApplicationsController(IApplicationService applications)
    {
        _applications = applications;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApplicationDto>> GetById(int id)
    {
        var app = await _applications.GetByIdAsync(id);
        return Ok(app);
    }

    [HttpGet("{id}/related/applicant")]
    public async Task<ActionResult<List<ApplicationDto>>> GetRelatedByApplicant(int id)
    {
        var res = await _applications.GetByApplicantAsync(id);
        return Ok(res);
    }

    [HttpGet("{id}/related/representative")]
    public async Task<ActionResult<List<ApplicationDto>>> GetRelatedByRepresentative(int id)
    {
        var res = await _applications.GetByRepresentativeAsync(id);
        return Ok(res);
    }

    [HttpGet("{id}/related/geo")]
    public async Task<ActionResult<List<ApplicationDto>>> GetRelatedByGeo(int id)
    {
        var res = await _applications.GetByGeoIntersectionAsync(id);
        return Ok(res);
    }
}
