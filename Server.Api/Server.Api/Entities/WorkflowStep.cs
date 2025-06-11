using System.ComponentModel.DataAnnotations.Schema;

namespace GovServices.Server.Entities
{
    public class WorkflowStep
    {
        public int Id { get; set; }
        public int WorkflowId { get; set; }
        public Workflow Workflow { get; set; }
        public string Name { get; set; }
        public int Sequence { get; set; }
        [InverseProperty("FromStep")]
        public ICollection<WorkflowTransition> OutgoingTransitions { get; set; }

        [InverseProperty("ToStep")]
        public ICollection<WorkflowTransition> IncomingTransitions { get; set; }
    }
}
