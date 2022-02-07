using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Main.LastPositionDevice
{
    public class NotificationsResponse: BasicResponse
    {

        public List<CredencialSpiderFleet.Models.Itineraries.NotificationsPriority> listNotifications { get; set; }
        public NotificationsResponse()
        {
            listNotifications = new List<CredencialSpiderFleet.Models.Itineraries.NotificationsPriority>();
        }
    }
}