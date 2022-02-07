using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.Main.Map
{
    public class GeneralTrips
    {
        public int IdTrip { get; set; }
        public string IdDevice { get; set; }
        public string begin_date { get; set; }
        public string end_date { get; set; }
        public string distance { get; set; }
        public string alarm { get; set; }
        public string fuel { get; set; }
    }
}