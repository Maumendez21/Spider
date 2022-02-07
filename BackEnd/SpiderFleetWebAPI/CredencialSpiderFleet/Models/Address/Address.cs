using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.Address
{
    public class Address
    {
        public string Device { get; set; }
        public DateTime Date { get; set; }
        public string Point { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}