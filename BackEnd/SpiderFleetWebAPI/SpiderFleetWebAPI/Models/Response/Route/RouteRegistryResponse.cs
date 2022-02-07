using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Route
{
    public class RouteRegistryResponse: BasicResponse
    {
        public CredencialSpiderFleet.Models.Route.Routes routes { get; set; }

        public RouteRegistryResponse()
        {
            routes = new CredencialSpiderFleet.Models.Route.Routes();
        }
    }
}