namespace GovServices.Server.DTOs;

public class DepartmentDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? ParentDepartmentId { get; set; }
    public string? Description { get; set; }
}
