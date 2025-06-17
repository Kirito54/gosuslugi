namespace GovServices.Server.Entities
{
    public class ApplicationRevision
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public Application Application { get; set; }
        public string Type { get; set; }
        public string DocumentNumber { get; set; }
        public string SedLink { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
