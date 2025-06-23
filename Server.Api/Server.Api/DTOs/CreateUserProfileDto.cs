namespace GovServices.Server.DTOs;

public class CreateUserProfileDto
{
    public string UserId { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public int PositionId { get; set; }
    public int DepartmentId { get; set; }
    public bool IsActive { get; set; } = true;
    public int? SupervisorId { get; set; }
}
