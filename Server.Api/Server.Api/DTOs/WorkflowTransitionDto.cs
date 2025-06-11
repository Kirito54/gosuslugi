namespace GovServices.Server.DTOs
{
    public class WorkflowTransitionDto
    {
        public int Id { get; set; }
        public int FromStepId { get; set; }
        public int ToStepId { get; set; }
        public string ConditionExpression { get; set; }
    }
}
