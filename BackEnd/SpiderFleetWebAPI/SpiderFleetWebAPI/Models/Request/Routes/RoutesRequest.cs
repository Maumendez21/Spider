using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Request.Routes
{
    public class RoutesRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        //public List<PointRoutes> ListRoutes { get; set; }
        public List<List<double>> ListPoints { get; set; }
    }
}