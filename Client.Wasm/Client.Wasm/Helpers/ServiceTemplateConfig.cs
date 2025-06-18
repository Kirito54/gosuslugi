using System.Text.Json;

namespace Client.Wasm.Helpers;

public class ServiceTemplateConfig
{
    public List<FieldConfig> Fields { get; set; } = new();
    public List<DocumentConfig> Documents { get; set; } = new();
    public List<string> ApplicantCategories { get; set; } = new();
    public List<WorkflowStepConfig> Workflow { get; set; } = new();

    public static ServiceTemplateConfig FromJson(string json)
    {
        if (string.IsNullOrWhiteSpace(json)) return new ServiceTemplateConfig();
        return JsonSerializer.Deserialize<ServiceTemplateConfig>(json) ?? new ServiceTemplateConfig();
    }

    public string ToJson() => JsonSerializer.Serialize(this);
}

public class FieldConfig
{
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = "text";
    public bool Required { get; set; }
    public int Order { get; set; }
    public string? Dictionary { get; set; }
}

public class DocumentConfig
{
    public string Name { get; set; } = string.Empty;
    public bool Required { get; set; }
}

public class WorkflowStepConfig
{
    public string Name { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public int Order { get; set; }
}
