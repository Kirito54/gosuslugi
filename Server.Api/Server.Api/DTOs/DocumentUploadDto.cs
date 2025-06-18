namespace GovServices.Server.DTOs;
using GovServices.Server.Entities;
using Microsoft.AspNetCore.Http;

public class DocumentUploadDto
{
    public DocumentType Type { get; set; }
    public Guid OwnerId { get; set; }
    public IFormFile File { get; set; } = default!;
    public VisibilityType Visibility { get; set; } = VisibilityType.InternalOnly;
}
