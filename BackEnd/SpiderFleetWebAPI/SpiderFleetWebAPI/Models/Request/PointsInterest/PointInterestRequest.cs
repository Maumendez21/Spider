using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Request.PointsInterest
{
    public class PointInterestRequest
    {
        public string Name { get; set; }
        public string Hierarchy { get; set; }
        public bool Active { get; set; }
        public string Description { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public double Radius { get; set; }

        //public List<List<double>> Coordinates { get; set; }
    }
}