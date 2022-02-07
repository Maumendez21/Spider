using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Itineraries
{
    public class ItinerariesResponse : BasicResponse
    {
        public List<CredencialSpiderFleet.Models.Itineraries.Itineraries> listItineraries { get; set; }

        public ItinerariesResponse()
        {
            listItineraries = new List<CredencialSpiderFleet.Models.Itineraries.Itineraries>();
        }
    }
}