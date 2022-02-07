using System;

namespace CredencialSpiderFleet.Models.ReportAdmin
{
    public class ReportGPS
    {
        public DateTime Date { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public double Speed { get; set; }
        public string RPM { get; set; }
    }
}