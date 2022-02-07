using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Main.GeoFence
{
    public class GeoFenceListHierarchyResponse : BasicResponse
    {
        public List<Mongo.GeoFence.GeoFences> listGeoFence { get; set; }

        public GeoFenceListHierarchyResponse()
        {
            listGeoFence = new List<Mongo.GeoFence.GeoFences>();
        }
    }
}