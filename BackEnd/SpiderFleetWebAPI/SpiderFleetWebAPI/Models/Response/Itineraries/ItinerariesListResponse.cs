using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Itineraries
{
    public class ItinerariesListResponse: BasicResponse
    {
        public List<List<CredencialSpiderFleet.Models.Itineraries.Itineraries>> listItineraries { get; set; }

        public ItinerariesListResponse()
        {
            listItineraries = new List<List<CredencialSpiderFleet.Models.Itineraries.Itineraries>>();
        }
    }
}