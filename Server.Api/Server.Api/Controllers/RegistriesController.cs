using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GovServices.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RegistriesController : ControllerBase
{
    [Authorize]
    [HttpGet("applications")]
    public IActionResult Applications() => Ok(new[] { new { Id = 1, Name = "Пример" } });

    [Authorize]
    [HttpGet("rdz-orders")]
    public IActionResult RdzOrders() => Ok(new[] { new { Id = 1, Number = "01-РДЗ" } });

    [Authorize]
    [HttpGet("rdi-orders")]
    public IActionResult RdiOrders() => Ok(new[] { new { Id = 1, Number = "01-РДИ" } });

    [Authorize]
    [HttpGet("answers")]
    public IActionResult Answers() => Ok(new[] { new { Id = 1, Subject = "Ответ" } });

    [Authorize]
    [HttpGet("contracts")]
    public IActionResult Contracts() => Ok(new[] { new { Id = 1, Name = "Договор" } });

    [Authorize]
    [HttpGet("acts")]
    public IActionResult Acts() => Ok(new[] { new { Id = 1, Name = "Акт" } });

    [Authorize]
    [HttpGet("agreements")]
    public IActionResult Agreements() => Ok(new[] { new { Id = 1, Name = "Соглашение" } });

    [Authorize(Roles = "Канцелярия")]
    [HttpGet("clerical")]
    public IActionResult Clerical() => Ok(new[] { new { Id = 1, Subject = "Получен ответ" } });
}
