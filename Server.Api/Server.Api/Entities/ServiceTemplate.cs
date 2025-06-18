using System;
namespace GovServices.Server.Entities;

public class ServiceTemplate
{
    public int Id { get; set; }
    public int ServiceId { get; set; }
    public Service Service { get; set; }
    public string JsonConfig { get; set; } = "{}";
    public bool IsActive { get; set; }
    public string UpdatedById { get; set; } = string.Empty;
    public ApplicationUser? UpdatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
}
