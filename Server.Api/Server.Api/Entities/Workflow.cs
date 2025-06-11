namespace GovServices.Server.Entities
{
    public class Workflow
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<WorkflowStep> Steps { get; set; }
    }
}
