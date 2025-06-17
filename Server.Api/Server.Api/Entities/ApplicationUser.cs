using Microsoft.AspNetCore.Identity;
namespace GovServices.Server.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public int? DepartmentId { get; set; }
        public Department Department { get; set; }
        public DateTime PasswordLastChangedAt { get; set; }
        public ICollection<Application> AssignedApplications { get; set; }
        public ICollection<UserPermissionGroup> PermissionGroups { get; set; }
    }
}
