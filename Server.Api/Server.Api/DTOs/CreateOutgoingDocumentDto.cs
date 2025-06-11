using Microsoft.AspNetCore.Http;
namespace GovServices.Server.DTOs
{
    public class CreateOutgoingDocumentDto
    {
        public int ApplicationId { get; set; }
        public IFormFile File { get; set; }
        public List<IFormFile> Attachments { get; set; }
    }
}
