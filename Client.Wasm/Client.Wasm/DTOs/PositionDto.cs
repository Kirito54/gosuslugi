namespace Client.Wasm.DTOs;

public class PositionDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
    public int StaffLimit { get; set; }
}
