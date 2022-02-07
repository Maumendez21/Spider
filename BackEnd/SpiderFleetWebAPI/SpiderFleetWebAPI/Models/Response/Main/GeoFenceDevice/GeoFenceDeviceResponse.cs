using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Main.GeoFenceDevice
{
    public class GeoFenceDeviceResponse : BasicResponse
    {
        public Dictionary<string, string> resultados { get; set; }

        public GeoFenceDeviceResponse()
        {
            resultados = new Dictionary<string, string>();
        }
    }
}