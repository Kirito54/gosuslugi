namespace GovServices.Server.DTOs
{
    public class SedDocumentLogDto
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public string DocumentNumber { get; set; }
        public string Action { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
