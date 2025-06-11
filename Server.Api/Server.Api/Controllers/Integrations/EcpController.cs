using Microsoft.AspNetCore.Mvc;
using GovServices.Server.Interfaces;

namespace GovServices.Server.Controllers.Integrations;

[ApiController]
[Route("api/integrations/[controller]")]
public class EcpController : ControllerBase
{
    private readonly IEcpService _ecp;

    public EcpController(IEcpService ecp)
    {
        _ecp = ecp;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok();
    }
}
