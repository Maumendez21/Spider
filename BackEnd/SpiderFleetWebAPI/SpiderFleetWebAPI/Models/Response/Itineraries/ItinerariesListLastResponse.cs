using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Itineraries
{
    public class ItinerariesListLastResponse : BasicResponse
    {
        public List<CredencialSpiderFleet.Models.Itineraries.Itineraries> listItineraries { get; set; }

        public ItinerariesListLastResponse()
        {
            listItineraries = new List<CredencialSpiderFleet.Models.Itineraries.Itineraries>();
        }
    }
}