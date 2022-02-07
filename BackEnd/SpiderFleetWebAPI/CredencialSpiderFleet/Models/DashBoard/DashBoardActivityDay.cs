using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.DashBoard
{
    public class DashBoardActivityDay
    {
        public string Device { get; set; }
        public string VehicleName { get; set; }
        public List<Itineraries.Itineraries> ListItineraries { get; set; }
    }
}