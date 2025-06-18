namespace Client.Wasm.DTOs;

public class NumberTemplateDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string TargetType { get; set; } = string.Empty;
    public string TemplateText { get; set; } = string.Empty;
    public ResetPolicy ResetPolicy { get; set; }
}
