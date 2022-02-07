using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.Main.Itineracios
{
    public class Desaceleracion
    {
        public int Id { get; set; }

        public string Vehiculo { get; set; }

        public DateTime? Fecha { get; set; }

        public string Latitud { get; set; }

        public string Longitud { get; set; }

        public int? Velocidad { get; set; }

        public double? Aceleracion { get; set; }

        public double? Desaceleracion1 { get; set; }

        public int? RPM { get; set; }

        public double? Rendimiento { get; set; }

        public string Nivel { get; set; }

        public double? AX { get; set; }

        public double? AY { get; set; }

        public double? AZ { get; set; }
    }
}