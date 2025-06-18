namespace GovServices.Server.Entities;

public enum ResetPolicy
{
    None,
    Daily,
    Monthly,
    Yearly,
    Manual
}

public class NumberTemplate
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string TargetType { get; set; } = string.Empty;
    public string TemplateText { get; set; } = string.Empty;
    public ResetPolicy ResetPolicy { get; set; }
}
