using Microsoft.AspNetCore.Components.Forms;
using Client.Wasm.DTOs;

namespace Client.Wasm.DTOs;

public class DocumentUploadDto
{
    public DocumentType Type { get; set; }
    public Guid OwnerId { get; set; }
    public IBrowserFile File { get; set; } = default!;
    public VisibilityType Visibility { get; set; } = VisibilityType.InternalOnly;
}
