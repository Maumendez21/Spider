using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Obd
{
    public class ObdRegistryResponse : BasicResponse
    {
        public CredencialSpiderFleet.Models.Obd.ObdRegistry obd { get; set; }

        public ObdRegistryResponse()
        {
            obd = new CredencialSpiderFleet.Models.Obd.ObdRegistry();
        }
    }
}