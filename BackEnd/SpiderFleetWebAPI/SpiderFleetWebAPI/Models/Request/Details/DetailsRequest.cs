using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Request.Details
{
    public class DetailsRequest
    {
        public string Device { get; set; }
        public int TypeDevice { get; set; }
        public string Model { get; set; }
        public int IdCommunicationMethod { get; set; }
        public int Batery { get; set; }
        public string BatteryDuration { get; set; }
        public int IdSamplingTime { get; set; }
        public int Motorized { get; set; }
        public double Performance { get; set; }
    }
}