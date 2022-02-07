using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Request.Routes
{
    public class RoutesConsolaRequest
    {
        //public string Name { get; set; }
        //public string Description { get; set; }
        public string Device { get; set; }
        public string IdRoute { get; set; }
        public DateTime Date { get; set; }
        //public List<PointRoutes> ListRoutes { get; set; }
        public List<double> ListPoints { get; set; }
    }
}