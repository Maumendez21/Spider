using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Inventory.TypeDevice
{
    public class TypeDeviceRegistryResponse :  BasicResponse
    {
        public CredencialSpiderFleet.Models.Inventory.TypeDevice.TypeDevices typeDevice { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TypeDeviceRegistryResponse()
        {
            typeDevice = new CredencialSpiderFleet.Models.Inventory.TypeDevice.TypeDevices();
        }
    }
}