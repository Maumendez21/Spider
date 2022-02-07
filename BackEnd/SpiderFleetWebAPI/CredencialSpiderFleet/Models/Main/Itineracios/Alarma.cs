using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.Main.Itineracios
{
    public class Alarma
    {
        public long Id { get; set; }

        public DateTime Fecha { get; set; }

        public string Device { get; set; }

        public string Evento { get; set; }

        public string Latitud { get; set; }

        public string Longitud { get; set; }

        public Decimal? Velocidad { get; set; }

        public Decimal? Direccion { get; set; }

        public int? TM { get; set; }

        public Decimal? TF { get; set; }

        public int? CM { get; set; }

        public Decimal? CF { get; set; }

        public DateTime? LastACCON { get; set; }

        public string Alarma1 { get; set; }

        public int? EA { get; set; }

        public string VA { get; set; }

        public string TA { get; set; }
    }
}