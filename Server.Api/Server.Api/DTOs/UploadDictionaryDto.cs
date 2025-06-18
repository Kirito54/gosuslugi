using Microsoft.AspNetCore.Http;

namespace GovServices.Server.DTOs;

public class UploadDictionaryDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string SourceType { get; set; } = "Manual";
    public string? SourceUrl { get; set; }
    public IFormFile File { get; set; } = default!;
}
