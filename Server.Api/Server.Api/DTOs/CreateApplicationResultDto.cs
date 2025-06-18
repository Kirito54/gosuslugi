namespace GovServices.Server.DTOs
{
    public class CreateApplicationResultDto
    {
        public int ApplicationId { get; set; }
        public Guid DocumentId { get; set; }
        public string Type { get; set; }
        public bool Automatic { get; set; }
    }
}
