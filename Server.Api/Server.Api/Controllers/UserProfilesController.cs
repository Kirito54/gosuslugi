using Microsoft.AspNetCore.Mvc;
using GovServices.Server.Interfaces;
using GovServices.Server.DTOs;

namespace GovServices.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserProfilesController : ControllerBase
{
    private readonly IUserProfileService _service;
    public UserProfilesController(IUserProfileService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<UserProfileDto>>> Get()
    {
        var items = await _service.GetAllAsync();
        return Ok(items);
    }
}
