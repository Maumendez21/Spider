using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Request.Main.GeoFenceDevice
{
    public class GeoFenceDeviceListIdRequest
    {
        public List<string> ListDeviceId { get; set; }
    }
}