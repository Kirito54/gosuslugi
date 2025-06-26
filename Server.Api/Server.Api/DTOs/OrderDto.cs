namespace GovServices.Server.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public int? ApplicationId { get; set; }
        public string OrderType { get; set; }
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public string? Preamble { get; set; }
        public string Text { get; set; }
        public string? CopiesTo { get; set; }
        public string SignerUserId { get; set; }
        public string SignerUserName { get; set; }
    }
}
