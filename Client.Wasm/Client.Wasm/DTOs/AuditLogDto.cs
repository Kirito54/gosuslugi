namespace Client.Wasm.DTOs
{
    public class AuditLogDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string ActionType { get; set; }
        public string EntityType { get; set; }
        public string EntityId { get; set; }
        public DateTime Timestamp { get; set; }
        public long DurationMs { get; set; }
    }
}
