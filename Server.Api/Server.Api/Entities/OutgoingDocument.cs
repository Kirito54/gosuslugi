namespace GovServices.Server.Entities
{
    public class OutgoingDocument
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public Application Application { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public DateTime Date { get; set; }
        public ICollection<OutgoingAttachment> Attachments { get; set; }
    }
}
