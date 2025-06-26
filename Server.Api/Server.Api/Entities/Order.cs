namespace GovServices.Server.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int? ApplicationId { get; set; }
        public Application Application { get; set; }
        public string OrderType { get; set; } // "RDZ" или "RDI"
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public string? Preamble { get; set; }
        public string Text { get; set; }
        public string? CopiesTo { get; set; }
        public string SignerUserId { get; set; }
        public ApplicationUser Signer { get; set; }
    }
}
