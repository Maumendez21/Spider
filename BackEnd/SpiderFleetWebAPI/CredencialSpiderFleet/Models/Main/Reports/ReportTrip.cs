using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.Main.Reports
{
    public class ReportTrip
    {
        public int id { get; set; }
        public string Dispositivo { get; set; }
        public string NombreOperador { get; set; }
        public DateTime I { get; set; }
        public string Inicio { get; set; }
        public string FI { get; set; }
        public string HI { get; set; }
        public string HF { get; set; }
        public DateTime F { get; set; }
        public string Fin { get; set; }
        public string Transcurrido { get; set; }
        public int Vel { get; set; }
        public int Rpm { get; set; }
        public int Ace { get; set; }
        public int Des { get; set; }
        public int ODO { get; set; }
        public Decimal Litros { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }
        public string RPMmax { get; set; }
        public string VELmax { get; set; }
        public string Ralenti { get; set; }
    }
}