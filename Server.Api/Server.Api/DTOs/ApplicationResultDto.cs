namespace GovServices.Server.DTOs
{
    public class ApplicationResultDto
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public int DocumentId { get; set; }
        public string Type { get; set; }
        public DateTime LinkedAt { get; set; }
        public bool Automatic { get; set; }
    }
}
