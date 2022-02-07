using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.ReportAdmin
{
    public class ReportLastStatusDevice
    {
        public string Login { get; set; }
        public string Nombre { get; set; }
        public string FechaLogin { get; set; }
        public string Mode { get; set; }
        public string GPS { get; set; }
        public string FechaGPS { get; set; }
        public string VersionSW { get; set; }
        public string VersionHW { get; set; }
    }
}