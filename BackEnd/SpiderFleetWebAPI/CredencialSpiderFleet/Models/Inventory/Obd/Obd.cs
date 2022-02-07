using System;

namespace CredencialSpiderFleet.Models.Inventory.Obd
{
    public class Obd
    {
        public string IdDevice { get; set; }
        public string Label { get; set; }
        public int? IdCompany { get; set; }
        public int IdType { get; set; }
        public int? IdSim { get; set; }
        public string SIM { get; set; }
        public string Name { get; set; }
        public string TaxID { get; set; }
    }
}