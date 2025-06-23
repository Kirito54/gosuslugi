namespace GovServices.Server.DTOs;

public class UpdateUserProfileDto
{
    public string FullName { get; set; } = string.Empty;
    public int PositionId { get; set; }
    public int DepartmentId { get; set; }
    public bool IsActive { get; set; }
    public int? SupervisorId { get; set; }
}
