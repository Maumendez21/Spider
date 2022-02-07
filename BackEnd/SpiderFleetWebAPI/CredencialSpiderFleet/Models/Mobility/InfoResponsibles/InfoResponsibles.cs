using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.Mobility.InfoResponsibles
{
    public class InfoResponsibles
    {
        public string Start { get; set; }
        public string End { get; set; }
        public string Notes { get; set; }
        public string Name { get; set; }
        public List<Itineraries.Itineraries> ListItineraries { get; set; }
        public int NumberTrips { get; set; }

        public InfoResponsibles()
        {            
            ListItineraries = new List<Itineraries.Itineraries>();
        }
    }
}