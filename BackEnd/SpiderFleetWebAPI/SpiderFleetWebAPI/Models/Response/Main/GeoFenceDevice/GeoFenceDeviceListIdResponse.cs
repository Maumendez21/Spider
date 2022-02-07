using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Main.GeoFenceDevice
{
    public class GeoFenceDeviceListIdResponse : BasicResponse
    {
        public Dictionary<string, string> resultados { get; set; }

        public GeoFenceDeviceListIdResponse()
        {
            resultados = new Dictionary<string, string>();
        }
    }
}