namespace Client.Wasm.DTOs
{
    public class ApplicationLogDto
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public string Action { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
