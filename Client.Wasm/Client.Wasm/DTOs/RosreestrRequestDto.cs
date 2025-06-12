namespace Client.Wasm.DTOs
{
    public class RosreestrRequestDto
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public string RequestId { get; set; }
        public string Status { get; set; }
        public string ResponseData { get; set; }
    }
}
