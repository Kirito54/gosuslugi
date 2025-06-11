using Microsoft.AspNetCore.Mvc;
using GovServices.Server.Interfaces;

namespace GovServices.Server.Controllers.Integrations;

[ApiController]
[Route("api/integrations/[controller]")]
public class SedController : ControllerBase
{
    private readonly ISedIntegrationService _sed;

    public SedController(ISedIntegrationService sed)
    {
        _sed = sed;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok();
    }
}
