using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.Main.HeatPoints
{
    public class HeatPoints
    {
        public string Device { get; set; }
        public DateTime Date { get; set; }
        public string Event { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}