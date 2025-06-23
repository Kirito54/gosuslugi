namespace GovServices.Server.Entities
{
    public class Service
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? ResponsibleDepartmentId { get; set; }
        public Department? ResponsibleDepartment { get; set; }
        public int? ExecutionDeadlineDays { get; set; }
        public DateTime? ExecutionDeadlineDate { get; set; }
        public string? ExecutionStagesJson { get; set; }
        public string Status { get; set; } = "В процессе";
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ICollection<Application> Applications { get; set; }
    }
}
