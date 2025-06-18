namespace Client.Wasm.DTOs;

public class CreateServiceTemplateDto
{
    public int ServiceId { get; set; }
    public string JsonConfig { get; set; } = "{}";
    public bool IsActive { get; set; }
}
