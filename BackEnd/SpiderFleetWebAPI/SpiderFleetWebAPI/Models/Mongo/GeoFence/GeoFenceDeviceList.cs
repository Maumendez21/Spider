using CredencialSpiderFleet.Models.Models.Mongo.GeoFence;
using System.Collections.Generic;

namespace SpiderFleetWebAPI.Models.Mongo.GeoFence
{
    public class GeoFenceDeviceList
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Hierarchy { get; set; }
        public Polygons Polygon { get; set; }
        public List<GeoFenceDeviceRegistry> ListDevice { get; set; }
    }
}