using CredencialSpiderFleet.Models.Models.Mongo.GeoFence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Mongo.PointInterest
{
    public class PointInterestData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Hierarchy { get; set; }
        public bool Active { get; set; }
        public string Description { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public double Radius { get; set; }
        //public Polygons Polygon { get; set; }
    }
}