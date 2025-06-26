namespace Client.Wasm.DTOs;

public class DocumentClassificationResultDto
{
    public string Type { get; set; } = string.Empty;
    public Dictionary<string, string> Fields { get; set; } = new();
}
