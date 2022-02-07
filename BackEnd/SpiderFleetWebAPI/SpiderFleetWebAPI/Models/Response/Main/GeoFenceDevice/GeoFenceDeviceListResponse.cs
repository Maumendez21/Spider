using SpiderFleetWebAPI.Models.Mongo.GeoFence;

namespace SpiderFleetWebAPI.Models.Response.Main.GeoFenceDevice
{
    public class GeoFenceDeviceListResponse : BasicResponse
    {
        public GeoFenceList fence { get; set; }

        public GeoFenceDeviceListResponse()
        {
            fence = new GeoFenceList();
        }
    }
}