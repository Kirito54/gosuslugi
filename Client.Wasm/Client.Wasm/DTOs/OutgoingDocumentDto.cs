namespace Client.Wasm.DTOs
{
    public class OutgoingDocumentDto
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public DateTime Date { get; set; }
        public List<string> AttachmentFileNames { get; set; }
    }
}
