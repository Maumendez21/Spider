using CredencialSpiderFleet.Models.Main.HeatPoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Main.HeatTrip
{
    public class HeatTripResponse : BasicResponse
    {
        public List<HeatPoints> listHeats { get; set; }

        public HeatTripResponse()
        {
            listHeats = new List<HeatPoints>();
        }
    }
}