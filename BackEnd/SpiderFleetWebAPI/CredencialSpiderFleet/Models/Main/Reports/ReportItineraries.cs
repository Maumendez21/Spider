using System;

namespace CredencialSpiderFleet.Models.Main.Reports
{
    public class ReportItineraries
    {
        public DateTime? Fecha { get; set; }
        public int? ODO { get; set; }
        public Decimal? Valor { get; set; }
        public int? DIFF { get; set; }
        public int? Company { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }
    }
}