using GovServices.Server.Entities;

namespace GovServices.Server.DTOs
{
    public class CreateApplicationDto
    {
        public int ServiceId { get; set; }
        public string? ExternalNumber { get; set; }
        public string ApplicantName { get; set; } = string.Empty;
        public string? RepresentativeName { get; set; }
        public string Address { get; set; } = string.Empty;
        public ApplicationSource Source { get; set; }
        public string? RegistrarId { get; set; }
        public Dictionary<string, object> FormData { get; set; }
    }
}
