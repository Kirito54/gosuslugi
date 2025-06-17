using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GovServices.Server.Interfaces;

namespace GovServices.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;

    public AuthController(IAuthService auth)
    {
        _auth = auth;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public IActionResult Login()
    {
        return Ok();
    }
}
