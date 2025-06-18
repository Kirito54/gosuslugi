namespace GovServices.Server.DTOs;

public class UpdateServiceTemplateDto
{
    public string JsonConfig { get; set; } = "{}";
    public bool IsActive { get; set; }
}
