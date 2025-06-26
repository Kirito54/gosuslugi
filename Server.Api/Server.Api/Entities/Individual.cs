namespace GovServices.Server.Entities;

public class Individual
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? IdentificationNumber { get; set; }
    public string? Address { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
