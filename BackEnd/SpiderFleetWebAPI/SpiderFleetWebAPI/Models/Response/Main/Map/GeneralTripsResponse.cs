using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Main.Map
{
    public class GeneralTripsResponse : BasicResponse
    {
        public List<CredencialSpiderFleet.Models.Main.Map.GeneralTrips> listTrips { get; set; }

        public GeneralTripsResponse()
        {
            listTrips = new List<CredencialSpiderFleet.Models.Main.Map.GeneralTrips>();
        }
    }
}