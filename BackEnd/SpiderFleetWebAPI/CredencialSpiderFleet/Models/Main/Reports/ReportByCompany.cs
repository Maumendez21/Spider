using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.Main.Reports
{
    public class ReportByCompany
    {
        public string Hierarchy { get; set; }
        public string Device { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Event { get; set; }
        public string Mode { get; set; }
        public string TypeAlarm { get; set; }
        public string StatusEvents { get; set; }
        public string Empresa { get; set; }
        public string Alarma { get; set; }
        public string Saldo { get; set; }
        public string VersionSW { get; set; }
        public string VersionHW { get; set; }
        public string IdSim { get; set; }
        public string Sim { get; set; }
    }
}