namespace Client.Wasm.DTOs
{
    public class UpdateApplicationDto
    {
        public int CurrentStepId { get; set; }
        public string Status { get; set; }
        public string AssignedToUserId { get; set; }
    }
}
