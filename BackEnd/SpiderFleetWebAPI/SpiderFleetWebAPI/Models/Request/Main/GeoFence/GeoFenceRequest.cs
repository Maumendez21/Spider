using System.Collections.Generic;

namespace SpiderFleetWebAPI.Models.Request.Main.GeoFence
{
    public class GeoFenceRequest
    {
        public string Name { get; set; }
        public string Hierarchy { get; set; }
        public bool Active { get; set; }
        public string Description { get; set; }
        public List<List<double>> Coordinates { get; set; }
    }
}