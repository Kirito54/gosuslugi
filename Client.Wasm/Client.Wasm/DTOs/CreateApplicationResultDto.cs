namespace Client.Wasm.DTOs
{
    public class CreateApplicationResultDto
    {
        public int ApplicationId { get; set; }
        public int DocumentId { get; set; }
        public string Type { get; set; }
        public bool Automatic { get; set; }
    }
}
