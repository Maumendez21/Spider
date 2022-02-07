using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Request.Main.GeoFenceDevice
{
    public class GeoFenceDeviceRequest
    {
        public string IdGeoFence { get; set; }
        public List<string> ListDevice { get; set; }
    }
}