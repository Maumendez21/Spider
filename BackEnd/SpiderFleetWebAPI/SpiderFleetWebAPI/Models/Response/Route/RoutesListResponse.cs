using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Route
{
    public class RoutesListResponse: BasicResponse
    {
        public List<CredencialSpiderFleet.Models.Route.Routes> routes { get; set; }

        public RoutesListResponse()
        {
            routes = new List<CredencialSpiderFleet.Models.Route.Routes>();
        }
    }
}