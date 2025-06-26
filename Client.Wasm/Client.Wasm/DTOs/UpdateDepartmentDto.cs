namespace Client.Wasm.DTOs;

public class UpdateDepartmentDto
{
    public string Name { get; set; } = string.Empty;
    public int? ParentDepartmentId { get; set; }
    public string? Description { get; set; }
}
