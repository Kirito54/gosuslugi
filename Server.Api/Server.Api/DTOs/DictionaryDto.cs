namespace GovServices.Server.DTOs;

public class DictionaryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime UpdatedAt { get; set; }
    public string SourceType { get; set; } = string.Empty;
    public int RecordCount { get; set; }
}
