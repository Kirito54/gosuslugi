namespace GovServices.Server.Entities
{
    public class ApplicationResult
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public Application Application { get; set; }
        public Guid DocumentId { get; set; }
        public Document Document { get; set; } = null!;
        public string Type { get; set; }
        public DateTime LinkedAt { get; set; }
        public bool Automatic { get; set; }
    }
}
