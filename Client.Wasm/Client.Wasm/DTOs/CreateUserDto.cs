namespace Client.Wasm.DTOs
{
    public class CreateUserDto
    {
        public string Email { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public int DepartmentId { get; set; }
        public int PositionId { get; set; }
        public HashSet<string> RoleIds { get; set; } = new();
        public HashSet<int> GroupIds { get; set; } = new();
    }
}
