namespace Client.Wasm.DTOs
{
    public class UpdateUserDto
    {
        public string FullName { get; set; }
        public int DepartmentId { get; set; }
        public List<string> RoleIds { get; set; }
    }
}
