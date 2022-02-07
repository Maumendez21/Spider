using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Request.Main.GeoFence
{
    public class GeoFenceIdRequest: GeoFenceRequest
    {
        public string Id { get; set; }
    }
}