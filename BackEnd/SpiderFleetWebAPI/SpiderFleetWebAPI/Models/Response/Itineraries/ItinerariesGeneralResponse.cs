using CredencialSpiderFleet.Models.Itineraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Itineraries
{
    public class ItinerariesGeneralResponse : BasicResponse
    {
        public CredencialSpiderFleet.Models.Itineraries.Itineraries Itineraries { get; set; }
        public List<LastAlarms> lastAlarms { get; set; }

        public ItinerariesGeneralResponse()
        {
            Itineraries = new CredencialSpiderFleet.Models.Itineraries.Itineraries();
            lastAlarms = new List<LastAlarms>();
        }
    }
}