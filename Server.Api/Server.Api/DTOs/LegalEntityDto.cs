namespace GovServices.Server.DTOs;

public class LegalEntityDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? TaxId { get; set; }
    public string? Address { get; set; }
}
