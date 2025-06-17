namespace GovServices.Server.Entities;

public class UserPermissionGroup
{
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
    public int PermissionGroupId { get; set; }
    public PermissionGroup PermissionGroup { get; set; }
}
