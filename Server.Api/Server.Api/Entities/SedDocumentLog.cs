namespace GovServices.Server.Entities
{
    public class SedDocumentLog
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public Application Application { get; set; }
        public string DocumentNumber { get; set; }
        public string Action { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
