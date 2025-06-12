namespace Client.Wasm.DTOs
{
    public class CreateApplicationDto
    {
        public int ServiceId { get; set; }
        public Dictionary<string, object> FormData { get; set; }
    }
}
