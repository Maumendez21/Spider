using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.ReportAdmin
{
    public class ReportDeviceLastData
    {
        public string Event { get; set; }
        public string Device { get; set; }
        public string Alarma { get; set; }        
        public DateTime Date { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Mode { get; set; }        
        public string VersionSW { get; set; }
        public string VersionHW { get; set; }
    }
}