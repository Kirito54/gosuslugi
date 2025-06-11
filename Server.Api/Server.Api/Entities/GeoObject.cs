using NetTopologySuite.Geometries;
namespace GovServices.Server.Entities
{
    public class GeoObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Geometry Geometry { get; set; }
        public string Properties { get; set; }
    }
}
