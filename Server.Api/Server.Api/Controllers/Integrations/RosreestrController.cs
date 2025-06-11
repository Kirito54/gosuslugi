using Microsoft.AspNetCore.Mvc;
using GovServices.Server.Interfaces;

namespace GovServices.Server.Controllers.Integrations;

[ApiController]
[Route("api/integrations/[controller]")]
public class RosreestrController : ControllerBase
{
    private readonly IRosreestrIntegrationService _rosreestr;

    public RosreestrController(IRosreestrIntegrationService rosreestr)
    {
        _rosreestr = rosreestr;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok();
    }
}
