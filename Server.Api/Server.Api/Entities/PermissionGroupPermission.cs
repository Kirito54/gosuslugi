namespace GovServices.Server.Entities;

public class PermissionGroupPermission
{
    public int PermissionGroupId { get; set; }
    public PermissionGroup PermissionGroup { get; set; }
    public int PermissionId { get; set; }
    public Permission Permission { get; set; }
}
