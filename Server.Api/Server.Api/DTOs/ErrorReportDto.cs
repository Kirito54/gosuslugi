namespace GovServices.Server.DTOs;

public class ErrorReportDto
{
    public string Message { get; set; } = string.Empty;
    public string? StackTrace { get; set; }
    public string? UserComment { get; set; }
    public string? UserId { get; set; }
}
