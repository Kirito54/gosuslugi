namespace Client.Wasm.DTOs;

public class CreatePermissionGroupDto
{
    public string Name { get; set; } = string.Empty;
    public List<int> PermissionIds { get; set; } = new();
}
