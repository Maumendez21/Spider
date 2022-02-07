using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Mongo.RouteConsola
{
    public class RouteData
    {
        public int IdRegistry { get; set; }
        public string Device { get; set; }
        public string IdRoute { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}