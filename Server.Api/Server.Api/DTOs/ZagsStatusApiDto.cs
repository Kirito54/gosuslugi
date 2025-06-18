namespace GovServices.Server.DTOs;

public class ZagsStatusApiDto
{
    public string ResponseId { get; set; } = string.Empty;
    public int State { get; set; }
    public string StatusCode { get; set; } = string.Empty;
    public string StatusMessage { get; set; } = string.Empty;
    public DataDto Data { get; set; } = new();

    public class DataDto
    {
        public string ResponceXml { get; set; } = string.Empty;
        public List<AttachmentDto> Attachments { get; set; } = new();
    }
}
