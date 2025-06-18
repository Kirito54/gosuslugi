namespace GovServices.Server.Entities;

public class NumberTemplateCounter
{
    public int Id { get; set; }
    public int TemplateId { get; set; }
    public NumberTemplate Template { get; set; } = default!;
    public string ScopeKey { get; set; } = string.Empty;
    public int CurrentValue { get; set; }
}
