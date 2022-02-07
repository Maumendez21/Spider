using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.Itineraries
{
    public class WaitTime
    {
        public string events { get; set; }
        public DateTime Date { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }
        public string time { get; set; }
        public int wastedTime { get; set; }
    }
}