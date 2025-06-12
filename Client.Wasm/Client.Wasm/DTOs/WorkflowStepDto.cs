namespace Client.Wasm.DTOs
{
    public class WorkflowStepDto
    {
        public int Id { get; set; }
        public int WorkflowId { get; set; }
        public string Name { get; set; }
        public int Sequence { get; set; }
    }
}
