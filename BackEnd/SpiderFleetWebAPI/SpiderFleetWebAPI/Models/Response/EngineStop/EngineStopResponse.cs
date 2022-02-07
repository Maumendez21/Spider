using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.EngineStop
{
    public class EngineStopResponse : BasicResponse
    {
        public List<CredencialSpiderFleet.Models.EngineStop.EngineStop> ListEngineStops { get; set; }

        public EngineStopResponse()
        {
            ListEngineStops = new List<CredencialSpiderFleet.Models.EngineStop.EngineStop>();
        }
    }
}