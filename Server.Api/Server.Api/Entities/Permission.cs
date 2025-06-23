namespace GovServices.Server.Entities;

public class Permission
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string Role { get; set; } = string.Empty;
    public bool CanView { get; set; }
    public bool CanEdit { get; set; }
    public bool CanApprove { get; set; }
    public bool CanDelete { get; set; }
    public int ServiceId { get; set; }
    public Service Service { get; set; } = default!;
    public int DepartmentId { get; set; }
    public Department Department { get; set; } = default!;
    public ICollection<PermissionGroupPermission> PermissionGroupPermissions { get; set; } = new List<PermissionGroupPermission>();
}
