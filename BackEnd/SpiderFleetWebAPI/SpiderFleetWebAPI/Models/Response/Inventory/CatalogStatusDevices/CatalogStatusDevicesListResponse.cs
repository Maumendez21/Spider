using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Inventory.CatalogStatusDevices
{
    public class CatalogStatusDevicesListResponse : BasicResponse
    {
        public List<CredencialSpiderFleet.Models.Inventory.CatalogStatusDevices.CatalogStatusDevices> listStatusDevices { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public CatalogStatusDevicesListResponse()
        {
            listStatusDevices = new List<CredencialSpiderFleet.Models.Inventory.CatalogStatusDevices.CatalogStatusDevices>();
        }
    }
}