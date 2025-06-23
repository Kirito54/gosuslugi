namespace Client.Wasm.DTOs;

public class UserProfileDto
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public int PositionId { get; set; }
    public int DepartmentId { get; set; }
    public bool IsActive { get; set; }
    public int? SupervisorId { get; set; }
}
