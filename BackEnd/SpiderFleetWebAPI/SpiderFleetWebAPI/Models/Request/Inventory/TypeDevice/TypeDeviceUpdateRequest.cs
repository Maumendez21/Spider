using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Request.Inventory.TypeDevice
{
    public class TypeDeviceUpdateRequest: TypeDeviceRequest
    {
        public int idTypeDevice { get; set; }
    }
}