using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.Logical
{
    public class RawData
    {
        public string Date { get; set; }
        public double Distance { get; set; }
        public string Seconds { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Travel { get; set; }
    }
}