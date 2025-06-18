namespace GovServices.Server.Entities;

public class ExtractRequest
{
    public int Id { get; set; }
    public int ServiceId { get; set; }
    public Application Service { get; set; }
    public string ExternalId { get; set; } = string.Empty;
    public RegistrySource RegistrySource { get; set; }
    public string Status { get; set; } = "Pending";
    public string ResponseRaw { get; set; } = string.Empty;
    public DateTime UpdatedAt { get; set; }
}
