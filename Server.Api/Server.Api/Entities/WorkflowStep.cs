namespace GovServices.Server.Entities
{
    public class WorkflowStep
    {
        public int Id { get; set; }
        public int WorkflowId { get; set; }
        public Workflow Workflow { get; set; }
        public string Name { get; set; }
        public int Sequence { get; set; }
        public ICollection<WorkflowTransition> OutgoingTransitions { get; set; }
        public ICollection<WorkflowTransition> IncomingTransitions { get; set; }
    }
}
