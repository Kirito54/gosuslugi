namespace GovServices.Server.Entities
{
    public class Document
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public Application Application { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public DateTime UploadedAt { get; set; }
        public string UploadedByUserId { get; set; }
        public ApplicationUser UploadedBy { get; set; }
        public DocumentMetadata Metadata { get; set; }
    }
}
