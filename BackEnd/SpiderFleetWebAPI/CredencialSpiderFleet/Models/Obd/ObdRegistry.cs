using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.Obd
{
    public class ObdRegistry
    {
        public string IdDevice { get; set; }
        public string Label { get; set; }
        public string Name { get; set; }
        public string Hierarchy { get; set; }
        public int? IdType { get; set; }        
        public string Description { get; set; }
        public int? IdSim { get; set; }
        public string Sim { get; set; }
        public int Status { get; set; }
        public int Motor { get; set; }
        public int Panico { get; set; }
    }
}