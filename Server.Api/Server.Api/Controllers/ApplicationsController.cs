using Microsoft.AspNetCore.Mvc;
using GovServices.Server.Interfaces;

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
}
