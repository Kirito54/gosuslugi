namespace GovServices.Server.Entities;

public class ZagsRequestAttachment
{
    public int Id { get; set; }
    public int ZagsRequestId { get; set; }
    public ZagsRequest ZagsRequest { get; set; }
    public string FileName { get; set; } = string.Empty;
    public byte[] Content { get; set; } = Array.Empty<byte>();
}
