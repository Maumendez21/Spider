using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.Sims
{
    public class SimsMaintenance
    {
        public int IdSim { get; set; }
        public string Sim { get; set; }
        public int Status { get; set; }
        public DateTime? LastUploadDate { get; set; }
    }
}