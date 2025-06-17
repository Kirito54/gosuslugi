namespace GovServices.Server.DTOs
{
    public class CreateApplicationRevisionDto
    {
        public int ApplicationId { get; set; }
        public string Type { get; set; }
        public string DocumentNumber { get; set; }
        public string SedLink { get; set; }
    }
}
