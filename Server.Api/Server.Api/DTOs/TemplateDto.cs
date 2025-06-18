namespace GovServices.Server.DTOs
{
    public class TemplateDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}
