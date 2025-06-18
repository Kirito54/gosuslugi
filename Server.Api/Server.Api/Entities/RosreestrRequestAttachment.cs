namespace GovServices.Server.Entities;

public class RosreestrRequestAttachment
{
    public int Id { get; set; }
    public int RosreestrRequestId { get; set; }
    public RosreestrRequest RosreestrRequest { get; set; }
    public string FileName { get; set; } = string.Empty;
    public byte[] Content { get; set; } = Array.Empty<byte>();
}
