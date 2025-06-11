using Microsoft.AspNetCore.Identity;

namespace Server.Api.Entities;

public class ApplicationUser : IdentityUser
{
    public Guid? DepartmentId { get; set; }
    public Department? Department { get; set; }
}
