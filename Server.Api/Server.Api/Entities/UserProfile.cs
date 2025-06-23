namespace GovServices.Server.Entities;

public class UserProfile
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = default!;
    public string FullName { get; set; } = string.Empty;
    public int PositionId { get; set; }
    public Position Position { get; set; } = default!;
    public int DepartmentId { get; set; }
    public Department Department { get; set; } = default!;
    public bool IsActive { get; set; } = true;
    public int? SupervisorId { get; set; }
    public UserProfile? Supervisor { get; set; }
    public ICollection<UserProfile> Subordinates { get; set; } = new List<UserProfile>();
}
