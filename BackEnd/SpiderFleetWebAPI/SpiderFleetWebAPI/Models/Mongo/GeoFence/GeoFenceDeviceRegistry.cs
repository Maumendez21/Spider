using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Mongo.GeoFence
{
    public class GeoFenceDeviceRegistry
    {
        public string Id { get; set; }
        public string Device { get; set; }
        public string Name { get; set; }
    }
}