using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.RouteAnalysis
{
    public class RouteAnalysis
    {
        public string Id { get; set; }
        public string Device { get; set; }
        public List<List<double>> Coordinates { get; set; }

        public RouteAnalysis()
        {
            Id = string.Empty;
            Device = string.Empty;
            Coordinates = new List<List<double>>();
        }
    }
}