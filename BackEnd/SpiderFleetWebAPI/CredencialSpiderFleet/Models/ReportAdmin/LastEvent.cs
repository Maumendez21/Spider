using System;

namespace CredencialSpiderFleet.Models.ReportAdmin
{
    public class LastEvent
    {
        public string Url { get; set; }
        public string Name { get; set; }
        public string Device { get; set; }
        public DateTime Date { get; set; }
        public string Event { get; set; }
    }
}