namespace Client.Wasm.DTOs
{
    public class TemplateDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        // Контент шаблона (HTML или Word)
        public string Content { get; set; }
    }
}
