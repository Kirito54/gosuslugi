namespace Client.Wasm.DTOs
{
    public class ApplicationDto
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public int CurrentStepId { get; set; }
        public string CurrentStepName { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string AssignedToUserId { get; set; }
        public string AssignedToUserName { get; set; }
    }
}
