namespace GovServices.Server.DTOs
{
    public class UpdateOrderDto
    {
        public string? Preamble { get; set; }
        public string Text { get; set; }
        public string? CopiesTo { get; set; }
        public string SignerUserId { get; set; }
    }
}
