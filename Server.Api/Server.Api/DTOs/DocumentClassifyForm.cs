namespace GovServices.Server.DTOs;

using Microsoft.AspNetCore.Http;

public class DocumentClassifyForm
{
    public string? Text { get; set; }
    public IFormFile? File { get; set; }
}
