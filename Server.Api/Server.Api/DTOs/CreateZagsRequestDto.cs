namespace GovServices.Server.DTOs
{
    public class CreateZagsRequestDto
    {
        public int ApplicationId { get; set; }
        public string ApplicantName { get; set; } = string.Empty;
        public string RepresentativeName { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public string RequestType { get; set; } = string.Empty;
    }
}
