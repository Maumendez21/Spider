using SpiderFleetWebAPI.Models.Mongo.GeoFence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Main.GeoFenceDevice
{
    public class LastStatusDeviceGeoFenceResponse :  BasicResponse
    {
        public List<GeoFenceDeviceLastPosition> ListLastStatusDevice { get; set; }

        public LastStatusDeviceGeoFenceResponse()
        {
            ListLastStatusDevice = new List<GeoFenceDeviceLastPosition>();
        }
    }
}