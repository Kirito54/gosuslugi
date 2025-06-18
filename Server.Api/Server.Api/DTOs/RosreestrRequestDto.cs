namespace GovServices.Server.DTOs;

public class RosreestrRequestDto
{
    public int Id { get; set; }
    public int ApplicationId { get; set; }
    public string RequestId { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string ResponseData { get; set; } = string.Empty;
    public List<AttachmentDto>? Attachments { get; set; }
}
