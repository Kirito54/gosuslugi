using Microsoft.AspNetCore.Components.Forms;
namespace Client.Wasm.DTOs
{
    public class CreateDocumentDto
    {
        public int ApplicationId { get; set; }
        public IBrowserFile File { get; set; }
    }
}
