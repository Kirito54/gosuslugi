namespace GovServices.Server.DTOs;

public class ServiceTemplateDto
{
    public int Id { get; set; }
    public int ServiceId { get; set; }
    public string? ServiceName { get; set; }
    public bool IsActive { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string? UpdatedByName { get; set; }
    public string JsonConfig { get; set; } = "{}";
}
