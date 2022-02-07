using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Catalog.Status
{
    public class StatusRegistryResponse: BasicResponse
    {
        public CredencialSpiderFleet.Models.Catalogs.Status.Status status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public StatusRegistryResponse()
        {
            status = new CredencialSpiderFleet.Models.Catalogs.Status.Status();
        }
    }
}