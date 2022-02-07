using System;

namespace CredencialSpiderFleet.Models.Obd
{
    public class ObdAdmin
    {
        public string Hierarchy { get; set; }
        public string Device { get; set; }
        public string VehicleName { get; set; }
        public string LastDate { get; set; }
        public string LastGPS { get; set; }
        public string Event { get; set; }
        public string LastDateAlarm { get; set; }
        public string LastAlarm { get; set; }
        public string LastDateLogin { get; set; }
        public string LastLogin { get; set; }
        public string Mode { get; set; }
        //public string LastDateSleep { get; set; }
        //public string LastSleep { get; set; }
        public string AlarmType { get; set; }
    }
}