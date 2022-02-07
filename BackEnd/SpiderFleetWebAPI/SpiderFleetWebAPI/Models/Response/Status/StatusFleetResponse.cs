using SpiderFleetWebAPI.Models.Mongo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Status
{
    public class StatusFleetResponse : BasicResponse
    {
        public List<InfoDevices> locations { get; set; }
        public StatusFleetResponse()
        {
            this.locations = new List<InfoDevices>();
        }
    }
}