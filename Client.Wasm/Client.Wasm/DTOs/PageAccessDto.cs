namespace Client.Wasm.DTOs;

public class PageAccessDto
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string PageUrl { get; set; } = string.Empty;
}
