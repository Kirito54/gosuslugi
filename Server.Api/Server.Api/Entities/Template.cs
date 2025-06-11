namespace GovServices.Server.Entities
{
    public class Template
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; } // "Word" или "HTML"
        public string Content { get; set; }
    }
}
