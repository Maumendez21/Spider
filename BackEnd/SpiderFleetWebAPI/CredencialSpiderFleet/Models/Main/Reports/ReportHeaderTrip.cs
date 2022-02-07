using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.Main.Reports
{
    public class ReportHeaderTrip
    {
        public int Viajes { get; set; }
        public string Litros { get; set; }
        public string ODO { get; set; }
        public string tiempo { get; set; }
        public int Vel { get; set; }
        public int Rpm { get; set; }
        public int Ace { get; set; }
        public int Des { get; set; }
        public string Rendimiento { get; set; }
    }
}