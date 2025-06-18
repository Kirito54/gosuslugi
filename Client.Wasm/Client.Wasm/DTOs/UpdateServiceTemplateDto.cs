namespace Client.Wasm.DTOs;

public class UpdateServiceTemplateDto
{
    public string JsonConfig { get; set; } = "{}";
    public bool IsActive { get; set; }
}
