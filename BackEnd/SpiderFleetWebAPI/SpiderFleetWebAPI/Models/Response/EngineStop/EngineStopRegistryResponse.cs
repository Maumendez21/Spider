using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.EngineStop
{
    public class EngineStopRegistryResponse :  BasicResponse
    {
        public CredencialSpiderFleet.Models.EngineStop.EngineStop EngineStop { get; set; }

        public EngineStopRegistryResponse()
        {
            EngineStop = new CredencialSpiderFleet.Models.EngineStop.EngineStop();
        }
    }
}