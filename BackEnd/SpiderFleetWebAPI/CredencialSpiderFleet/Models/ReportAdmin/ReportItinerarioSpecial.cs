using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.ReportAdmin
{
    public class ReportItinerarioSpecial
    {
        public int Viaje { get; set; }
        public string Fecha { get; set; }
        public string Inicio { get; set; }
        public string Fin { get; set; }
        public string Tiempo { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public Decimal Velocidad { get; set; }
        public int Distancia { get; set; }
    }
}