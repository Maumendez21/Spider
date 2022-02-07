using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.ReportAdmin
{
    public class ReportItinerario
    {
        public int Viaje { get; set; }
        public string Fecha { get; set; }
        public string Inicio { get; set; }
        public string Fin { get; set; }
        public int Tiempo { get; set; }
        public int Aceleracion { get; set; }
        public int Frenado { get; set; }
        public int RPM { get; set; }
        public int Velocidad { get; set; }
        public int Distancia { get; set; }
        public double Consumo { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }
        public string Responsable { get; set; }
        public int Dongle { get; set; }
        public decimal Gas { get; set; }
        public decimal Km { get; set; }

    }
}