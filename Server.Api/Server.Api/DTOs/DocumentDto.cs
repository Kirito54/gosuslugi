namespace GovServices.Server.DTOs
{
    public class DocumentDto
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public DateTime UploadedAt { get; set; }
        public string UploadedByUserId { get; set; }
        public string UploadedByUserName { get; set; }
        public string MetadataJson { get; set; }
    }
}
