using CredencialSpiderFleet.Models.RouteDiary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.RouteDiary
{
    public class ListRouteDataResponse :  BasicResponse
    {

        public List<RouteData> ListRoutes { get; set; }

        public ListRouteDataResponse()
        {
            ListRoutes = new List<RouteData>();
        }
    }
}