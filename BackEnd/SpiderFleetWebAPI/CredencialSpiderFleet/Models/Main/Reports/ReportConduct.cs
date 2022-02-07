using System;

namespace CredencialSpiderFleet.Models.Main.Reports
{
    public class ReportConduct
    {
        public string Device { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public double Speed { get; set; }
        public string Event { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string NameSubCompany { get; set; }
    }
}