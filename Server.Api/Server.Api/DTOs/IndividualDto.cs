namespace GovServices.Server.DTOs;

public class IndividualDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? IdentificationNumber { get; set; }
    public string? Address { get; set; }
}
