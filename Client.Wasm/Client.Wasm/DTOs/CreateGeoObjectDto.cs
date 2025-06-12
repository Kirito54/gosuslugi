namespace Client.Wasm.DTOs
{
    public class CreateGeoObjectDto
    {
        public string Name { get; set; }
        public string WktGeometry { get; set; }
        public string Properties { get; set; }
    }
}
