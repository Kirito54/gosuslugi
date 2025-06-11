namespace GovServices.Server.Entities
{
    public class OutgoingAttachment
    {
        public int Id { get; set; }
        public int OutgoingDocumentId { get; set; }
        public OutgoingDocument OutgoingDocument { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }
}
