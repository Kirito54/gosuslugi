namespace GovServices.Server.Entities;

public class ErrorReport
{
    public int Id { get; set; }
    public string? UserId { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public string? StackTrace { get; set; }
    public string? UserComment { get; set; }
    public DateTime Timestamp { get; set; }
}
