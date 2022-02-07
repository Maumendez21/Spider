
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.Mobility.PointInterestAnalysis
{
    public class PointInterestAnalysis
    {
        public string Device { get; set; }
        public string VehicleName { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public bool Status { get; set; }
        public string Time { get; set; }
        public DateTime Date { get; set; }
        public int Diff { get; set; }
    }
}