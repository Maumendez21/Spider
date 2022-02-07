using System;

namespace CredencialSpiderFleet.Models.Inventory.Sim
{
    public class Sims
    {
        public int IdSim { get; set; }
        public string Sim { get; set; }
        public int Status { get; set; }
        public DateTime? LastUploadDate { get; set; }
    }
}