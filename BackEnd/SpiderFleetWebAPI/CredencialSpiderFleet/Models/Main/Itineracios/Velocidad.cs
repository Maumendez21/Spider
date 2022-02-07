using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.Main.Itineracios
{
    public class Velocidad
    {
        public int Id { get; set; }

        public string Vehiculo { get; set; }

        public DateTime? Fecha { get; set; }

        public string Latitud { get; set; }

        public string Longitud { get; set; }

        public double? Velocidad1 { get; set; }
    }
}