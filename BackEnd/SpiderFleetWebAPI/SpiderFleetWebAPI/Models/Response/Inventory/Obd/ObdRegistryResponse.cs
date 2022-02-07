using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Inventory.Obd
{
    public class ObdRegistryResponse: BasicResponse
    {
        public CredencialSpiderFleet.Models.Obd.Obd obd { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ObdRegistryResponse()
        {
            obd = new CredencialSpiderFleet.Models.Obd.Obd();
        }
    }
}