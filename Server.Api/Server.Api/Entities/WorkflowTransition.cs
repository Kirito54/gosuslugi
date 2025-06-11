namespace GovServices.Server.Entities
{
    public class WorkflowTransition
    {
        public int Id { get; set; }
        public int FromStepId { get; set; }
        public WorkflowStep FromStep { get; set; }
        public int ToStepId { get; set; }
        public WorkflowStep ToStep { get; set; }
        public string ConditionExpression { get; set; }
    }
}
