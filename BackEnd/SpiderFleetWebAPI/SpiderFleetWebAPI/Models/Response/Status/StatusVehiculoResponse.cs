using SpiderFleetWebAPI.Models.Mongo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Status
{
    public class StatusVehiculoResponse : BasicResponse
    {
        public InfoDevices location { get; set; }

        public StatusVehiculoResponse()
        {
            this.location = new InfoDevices();
        }
    }
}