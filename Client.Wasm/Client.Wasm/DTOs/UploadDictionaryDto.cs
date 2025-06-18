using Microsoft.AspNetCore.Components.Forms;

namespace Client.Wasm.DTOs;

public class UploadDictionaryDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string SourceType { get; set; } = "Manual";
    public string? SourceUrl { get; set; }
    public IBrowserFile File { get; set; } = default!;
}
