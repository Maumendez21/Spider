using CredencialSpiderFleet.Models.Models.Mongo.GeoFence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Request.Routes
{
    public class PointRoutes
    {
        public string Name { get; set; }
        public string Notes { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public List<List<double>> Coordinates { get; set; }
    }
}