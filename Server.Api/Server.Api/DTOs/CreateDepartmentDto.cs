namespace GovServices.Server.DTOs;

public class CreateDepartmentDto
{
    public string Name { get; set; } = string.Empty;
    public int? ParentDepartmentId { get; set; }
    public string? Description { get; set; }
}
