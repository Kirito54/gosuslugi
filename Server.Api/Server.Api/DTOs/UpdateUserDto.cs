namespace GovServices.Server.DTOs
{
    public class UpdateUserDto
    {
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public int DepartmentId { get; set; }
        public int PositionId { get; set; }
        public List<string> RoleIds { get; set; } = new();
        public List<int> GroupIds { get; set; } = new();
    }
}
