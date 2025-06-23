namespace GovServices.Server.DTOs
{
    public class ServiceDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? ExecutionDeadlineDays { get; set; }
        public DateTime? ExecutionDeadlineDate { get; set; }
        public List<ExecutionStage>? ExecutionStages { get; set; }
        public string Status { get; set; } = "В процессе";
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
