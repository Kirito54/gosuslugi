using Microsoft.AspNetCore.Mvc;
using GovServices.Server.Interfaces;

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
    public IActionResult GetAll()
    {
        return Ok();
    }
}
