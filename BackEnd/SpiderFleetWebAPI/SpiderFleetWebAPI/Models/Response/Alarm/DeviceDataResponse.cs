using CredencialSpiderFleet.Models.Logical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Alarm
{
    public class DeviceDataResponse : BasicResponse
    {
        public List<DeviceData> ListDevices { get; set; }

        public DeviceDataResponse()
        {
            ListDevices = new List<DeviceData>();
        }

    }
}