using GovServices.Server.Entities;

namespace GovServices.Server.DTOs
{
    public class ApplicationDto
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string? ExternalNumber { get; set; }
        public string ApplicantName { get; set; }
        public string? RepresentativeName { get; set; }
        public string Address { get; set; }
        public ApplicationSource Source { get; set; }
        public string? RegistrarId { get; set; }
        public string? RegistrarName { get; set; }
        public int CurrentStepId { get; set; }
        public string CurrentStepName { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string AssignedToUserId { get; set; }
        public string AssignedToUserName { get; set; }
    }
}
