using Microsoft.AspNetCore.Mvc;
using GovServices.Server.Interfaces;

namespace GovServices.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GeoObjectsController : ControllerBase
{
    private readonly IGeoService _geo;

    public GeoObjectsController(IGeoService geo)
    {
        _geo = geo;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok();
    }
}
