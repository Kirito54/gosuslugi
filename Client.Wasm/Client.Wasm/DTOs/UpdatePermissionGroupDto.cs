namespace Client.Wasm.DTOs;

public class UpdatePermissionGroupDto
{
    public string Name { get; set; } = string.Empty;
    public HashSet<int> PermissionIds { get; set; } = new();
}
