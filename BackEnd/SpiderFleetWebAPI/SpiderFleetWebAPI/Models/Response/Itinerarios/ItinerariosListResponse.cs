using SpiderFleetWebAPI.Models.Mongo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Itinerarios
{
    public class ItinerariosListResponse : BasicResponse
    {
        public List<Trips> storedTrips { get; set; }

        public ItinerariosListResponse()
        {
            this.storedTrips = new List<Trips>();
        }
    }
}