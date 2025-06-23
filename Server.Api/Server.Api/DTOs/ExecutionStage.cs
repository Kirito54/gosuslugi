namespace GovServices.Server.DTOs;

public class ExecutionStage
{
    public string Name { get; set; } = string.Empty;
    public int Days { get; set; }
    public string? ConditionJson { get; set; }
}
