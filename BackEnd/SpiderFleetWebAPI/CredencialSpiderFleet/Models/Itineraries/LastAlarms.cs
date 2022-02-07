using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.Itineraries
{
    public class LastAlarms
    {
        public string Alarm { get; set; }
        public DateTime Date { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}