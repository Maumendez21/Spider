using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Request.Itinearios
{
    public class ItinerariosListRequest
    {
        public string idVehiculo { get; set; }
        public DateTime dateInit { get; set; }
        public DateTime dateEnd { get; set; }

    }
}