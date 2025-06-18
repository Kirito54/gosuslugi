namespace Client.Wasm.DTOs
{
    public class ZagsRequestDto
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public string RequestId { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string ResponseXml { get; set; } = string.Empty;
    }
}
