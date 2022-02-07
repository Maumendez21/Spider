using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Inventory.TypeDevice
{
    public class TypeDeviceListResponse : BasicResponse
    {
        public List<CredencialSpiderFleet.Models.Inventory.TypeDevice.TypeDevices> listTypeDevice { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TypeDeviceListResponse()
        {
            listTypeDevice = new List<CredencialSpiderFleet.Models.Inventory.TypeDevice.TypeDevices>();
        }
    }
}