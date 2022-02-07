using CredencialSpiderFleet.Models.Main.LastPositionDevice;
using System.Collections.Generic;

namespace SpiderFleetWebAPI.Models.Response.Main.LastPositionDevice
{
    public class LastPositionDeviceResponse : BasicResponse
    {
        public List<CurrentPositionDevice> ListLastPosition { get; set; }
        public List<CredencialSpiderFleet.Models.Itineraries.NotificationsPriority> ListNotificationsPriority { get; set; }
        public int View { get; set; }
        public LastPositionDeviceResponse()
        {
            ListLastPosition = new List<CurrentPositionDevice>();
            ListNotificationsPriority = new List<CredencialSpiderFleet.Models.Itineraries.NotificationsPriority>();
            View = 0;
        }
    }
}