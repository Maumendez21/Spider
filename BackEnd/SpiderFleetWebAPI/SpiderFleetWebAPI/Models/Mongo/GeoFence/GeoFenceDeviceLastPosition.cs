using System;

namespace SpiderFleetWebAPI.Models.Mongo.GeoFence
{
    public class GeoFenceDeviceLastPosition
    {
        public string Device { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public int StatusEvent { get; set; }
    }
}