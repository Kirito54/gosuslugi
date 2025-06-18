using GovServices.Server.Data;
using GovServices.Server.DTOs;
using GovServices.Server.Entities;
using GovServices.Server.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GovServices.Server.Controllers;

[ApiController]
[Route("api/errors")]
public class ErrorReportController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IEmailService _email;
    private readonly IConfiguration _config;

    public ErrorReportController(ApplicationDbContext context, IEmailService email, IConfiguration config)
    {
        _context = context;
        _email = email;
        _config = config;
    }

    [HttpPost("report")]
    public async Task<IActionResult> Report(ErrorReportDto dto)
    {
        var report = new ErrorReport
        {
            ErrorMessage = dto.Message,
            StackTrace = dto.StackTrace,
            UserComment = dto.UserComment,
            UserId = dto.UserId,
            Timestamp = DateTime.UtcNow
        };
        _context.ErrorReports.Add(report);
        await _context.SaveChangesAsync();

        var adminEmail = _config.GetValue<string>("AdminEmail") ?? "admin@gosuslugi.local";
        var subject = $"Ошибка в системе от пользователя {dto.UserId}";
        var body = $"<p>{dto.Message}</p><pre>{dto.StackTrace}</pre><p>{dto.UserComment}</p>";
        await _email.SendEmailAsync(adminEmail, subject, body);

        return Ok();
    }
}
