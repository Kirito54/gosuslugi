using Microsoft.AspNetCore.Components.Forms;
namespace Client.Wasm.DTOs
{
    public class CreateOutgoingDocumentDto
    {
        public int ApplicationId { get; set; }
        public IBrowserFile File { get; set; }
        public List<IBrowserFile> Attachments { get; set; }
    }
}
