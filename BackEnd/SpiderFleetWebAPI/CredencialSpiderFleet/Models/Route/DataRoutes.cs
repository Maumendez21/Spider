using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.Route
{
    public class DataRoutes
    {
        public string Name { get; set; }
        public string Notes { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public List<List<double>> Coordinates { get; set; }
    }
}