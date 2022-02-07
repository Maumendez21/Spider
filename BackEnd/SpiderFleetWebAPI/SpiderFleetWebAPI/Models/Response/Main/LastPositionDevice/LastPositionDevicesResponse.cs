using CredencialSpiderFleet.Models.Main.LastPositionDevice;
using System.Collections.Generic;

namespace SpiderFleetWebAPI.Models.Response.Main.LastPositionDevice
{
    public class LastPositionDevicesResponse : BasicResponse
    {
        public List<LastPositionDevices> wp { get; set; }

        public LastPositionDevicesResponse()
        {
            wp = new List<LastPositionDevices>();
        }
    }
}