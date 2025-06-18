namespace GovServices.Server.DTOs
{
    public class UserDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public List<string> Roles { get; set; }
        public string DepartmentName { get; set; }
        public List<string> Groups { get; set; }
    }
}
