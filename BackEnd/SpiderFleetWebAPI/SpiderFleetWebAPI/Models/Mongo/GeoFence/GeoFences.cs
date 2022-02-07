using CredencialSpiderFleet.Models.Models.Mongo.GeoFence;

namespace SpiderFleetWebAPI.Models.Mongo.GeoFence
{
    public class GeoFences
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Hierarchy { get; set; }
        public bool Active { get; set; }
        public string Description { get; set; }
        public Polygons Polygon { get; set; }
    }
}