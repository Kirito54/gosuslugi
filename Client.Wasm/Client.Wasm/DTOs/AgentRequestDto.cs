using System.ComponentModel.DataAnnotations;

namespace Client.Wasm.DTOs;

public class AgentRequestDto
{
    [Required]
    public string Text { get; set; } = string.Empty;
}
