using System.ComponentModel.DataAnnotations;

namespace Client.Wasm.DTOs
{
    public class ChangePasswordDto
    {
        [Required]
        public string OldPassword { get; set; } = string.Empty;
        [Required]
        public string NewPassword { get; set; } = string.Empty;
    }
}
