using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Main.GeoFence
{
    public class GeoFenceRegistryResponse : BasicResponse
    {
        public Mongo.GeoFence.GeoFences GeoFence { get; set; }

        public GeoFenceRegistryResponse()
        {
            GeoFence = new Mongo.GeoFence.GeoFences();
        }
    }
}