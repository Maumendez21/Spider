using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Request.Routes
{
    public class RouteDeviceRequest
    {
        public string IdRoute { get; set; }
        public List<string> ListDevice { get; set; }
    }
}