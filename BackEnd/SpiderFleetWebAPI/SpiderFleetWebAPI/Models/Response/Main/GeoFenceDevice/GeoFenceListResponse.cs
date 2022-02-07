using SpiderFleetWebAPI.Models.Mongo.GeoFence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Main.GeoFenceDevice
{
    public class GeoFenceListResponse : BasicResponse
    {
        public List<GeoFences> ListGeoFences  { get; set; }

        public GeoFenceListResponse()
        {
            ListGeoFences = new List<GeoFences>();
        }
    }
}