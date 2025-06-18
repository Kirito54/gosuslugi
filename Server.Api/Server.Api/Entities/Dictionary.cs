namespace GovServices.Server.Entities;

public class Dictionary
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime UpdatedAt { get; set; }
    public string Schema { get; set; } = string.Empty;
    public string SourceType { get; set; } = "Manual";
    public string? SourceUrl { get; set; }
    public string Data { get; set; } = "[]"; // JSON array of records
}
