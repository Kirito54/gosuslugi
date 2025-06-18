namespace Client.Wasm.DTOs;

public class DocumentDto
{
    public Guid Id { get; set; }
    public DocumentType Type { get; set; }
    public Guid OwnerId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string OriginalName { get; set; } = string.Empty;
    public string MimeType { get; set; } = string.Empty;
    public string StoragePath { get; set; } = string.Empty;
    public string Hash { get; set; } = string.Empty;
    public string CreatedByUserId { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public VisibilityType Visibility { get; set; }
    public DocumentStatusType DocumentStatus { get; set; }
    public string? LinkedSEDId { get; set; }
}
