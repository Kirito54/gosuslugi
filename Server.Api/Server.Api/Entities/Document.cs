namespace GovServices.Server.Entities;

/// <summary>
/// Represents a stored document with metadata and versioning support.
/// </summary>
public class Document
{
    public Guid Id { get; set; }

    /// <summary>Business type of the document.</summary>
    public DocumentType Type { get; set; }

    /// <summary>Identifier of the related entity (application, order, etc.).</summary>
    public Guid OwnerId { get; set; }

    /// <summary>Unique file name on disk.</summary>
    public string FileName { get; set; } = string.Empty;

    /// <summary>Original file name provided by user.</summary>
    public string OriginalName { get; set; } = string.Empty;

    public string MimeType { get; set; } = string.Empty;

    /// <summary>Full storage path for the document.</summary>
    public string StoragePath { get; set; } = string.Empty;

    /// <summary>SHA-256 hash of file content.</summary>
    public string Hash { get; set; } = string.Empty;

    public string CreatedByUserId { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public VisibilityType Visibility { get; set; }

    public DocumentStatusType DocumentStatus { get; set; }

    public string? LinkedSEDId { get; set; }
}
