using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.Route
{
    public class Routes
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        //public List<DataRoutes> ListRoutes { get; set; }
        public List<List<double>> ListPoints { get; set; }
    }
}