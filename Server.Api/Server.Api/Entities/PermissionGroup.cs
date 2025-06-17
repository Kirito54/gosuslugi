namespace GovServices.Server.Entities;

public class PermissionGroup
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<PermissionGroupPermission> PermissionGroupPermissions { get; set; }
    public ICollection<UserPermissionGroup> UserPermissionGroups { get; set; }
}
