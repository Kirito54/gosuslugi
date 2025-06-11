using Microsoft.AspNetCore.Mvc;
using GovServices.Server.Interfaces;

namespace GovServices.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OutgoingController : ControllerBase
{
    private readonly IOutgoingService _outgoing;

    public OutgoingController(IOutgoingService outgoing)
    {
        _outgoing = outgoing;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok();
    }
}
