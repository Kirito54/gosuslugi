using Microsoft.AspNetCore.Http;
namespace GovServices.Server.DTOs
{
    public class CreateDocumentDto
    {
        public int ApplicationId { get; set; }
        public IFormFile File { get; set; }
    }
}
