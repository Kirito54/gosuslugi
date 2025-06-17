namespace GovServices.Server.DTOs
{
    public class UpdateUserDto
    {
        public string FullName { get; set; }
        public int DepartmentId { get; set; }
        public List<string> RoleIds { get; set; }
        public List<int> GroupIds { get; set; }
    }
}
