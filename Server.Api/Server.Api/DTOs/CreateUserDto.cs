namespace GovServices.Server.DTOs
{
    public class CreateUserDto
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        public int DepartmentId { get; set; }
        public List<string> RoleIds { get; set; }
    }
}
