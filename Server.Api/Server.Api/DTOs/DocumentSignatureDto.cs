namespace GovServices.Server.DTOs;

public class DocumentSignatureDto
{
    public Guid DocumentId { get; set; }
    public string SignatureBase64 { get; set; } = string.Empty;
}
