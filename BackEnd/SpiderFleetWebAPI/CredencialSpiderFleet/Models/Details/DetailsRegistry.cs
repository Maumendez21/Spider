using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.Details
{
    public class DetailsRegistry
    {
        public string Device { get; set; }
        public string Name { get; set; }
        public int TypeDevice { get; set; }
        public string DescriptionType { get; set; }
        public string Model { get; set; }
        public int IdCommunicationMethod { get; set; }
        public string DescriptionCommunicationMethod { get; set; }
        public int Batery { get; set; }
        public string BatteryDuration { get; set; }
        public int IdSamplingTime { get; set; }
        public string DescriptionSamplingTime { get; set; }
        public int Motorized { get; set; }
        public double Performance { get; set; }
        //public int Status { get; set; }
    }
}