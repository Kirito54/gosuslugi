namespace GovServices.Server.Entities
{
    public class DocumentMetadata
    {
        public int Id { get; set; }
        public int DocumentId { get; set; }
        public Document Document { get; set; }
        public string MetadataJson { get; set; }
    }
}
