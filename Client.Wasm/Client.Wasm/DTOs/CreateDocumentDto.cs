using Microsoft.AspNetCore.Http;
namespace Client.Wasm.DTOs
{
    public class CreateDocumentDto
    {
        public int ApplicationId { get; set; }
        public IFormFile File { get; set; }
    }
}
