using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.Main.Reports
{
    public class Reports
    {
        public DateTime FechaInicial { get; set; }
        public DateTime FechaFin { get; set; }
        public bool Trips { get; set; }
        public bool GeoFence { get; set; }
        public bool DrivingBehaviors { get; set; }
        public bool DataSources { get; set; }
    }
}