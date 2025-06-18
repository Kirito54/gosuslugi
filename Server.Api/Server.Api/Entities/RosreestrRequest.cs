namespace GovServices.Server.Entities;

public class RosreestrRequest
{
    public int Id { get; set; }
    public int ApplicationId { get; set; }
    public Application Application { get; set; }
    public string RequestId { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string ResponseData { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public ICollection<RosreestrRequestAttachment> Attachments { get; set; } = new List<RosreestrRequestAttachment>();
}
