namespace Client.Wasm.DTOs
{
    public class CreateOrderDto
    {
        public int? ApplicationId { get; set; }
        public string OrderType { get; set; }
        public string Text { get; set; }
        public string SignerUserId { get; set; }
    }
}
