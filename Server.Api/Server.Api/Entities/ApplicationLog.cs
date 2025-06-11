namespace GovServices.Server.Entities
{
    public class ApplicationLog
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public Application Application { get; set; }
        public string Action { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
