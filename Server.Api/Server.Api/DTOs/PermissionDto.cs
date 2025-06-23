namespace GovServices.Server.DTOs;

public class PermissionDto
{
    public int Id { get; set; }
    public string Role { get; set; } = string.Empty;
    public bool CanView { get; set; }
    public bool CanEdit { get; set; }
    public bool CanApprove { get; set; }
    public bool CanDelete { get; set; }
    public int ServiceId { get; set; }
    public int DepartmentId { get; set; }
}
