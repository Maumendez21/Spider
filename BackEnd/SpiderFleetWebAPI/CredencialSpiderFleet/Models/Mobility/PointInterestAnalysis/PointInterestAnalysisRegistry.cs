using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.Mobility.PointInterestAnalysis
{
    public class PointInterestAnalysisRegistry
    {
        public string Device { get; set; }
        public string VehicleName { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Time { get; set; }        
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Date { get; set; }
    }
}