namespace GovServices.Server.Entities;

public class Position
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
    public Department Department { get; set; } = default!;
    public int StaffLimit { get; set; }
    public ICollection<UserProfile> UserProfiles { get; set; } = new List<UserProfile>();
}
