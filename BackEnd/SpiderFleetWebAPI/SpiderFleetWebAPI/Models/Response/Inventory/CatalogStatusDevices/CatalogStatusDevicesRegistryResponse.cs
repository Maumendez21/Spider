using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Inventory.CatalogStatusDevices
{
    public class CatalogStatusDevicesRegistryResponse : BasicResponse
    {
        public CredencialSpiderFleet.Models.Inventory.CatalogStatusDevices.CatalogStatusDevices statusDevice { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public CatalogStatusDevicesRegistryResponse()
        {
            statusDevice = new CredencialSpiderFleet.Models.Inventory.CatalogStatusDevices.CatalogStatusDevices();
        }
    }
}