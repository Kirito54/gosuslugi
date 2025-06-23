namespace GovServices.Server.DTOs;

public class CreatePositionDto
{
    public string Name { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
    public int StaffLimit { get; set; }
}
