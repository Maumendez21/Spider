using System.Collections.Generic;

namespace CredencialSpiderFleet.Models.Models.Mongo.GeoFence
{
    public class Polygons
    {
        public string Type { get; set; }
        public List<CoordenatesData> Coordinates { get; set; }
    }
}