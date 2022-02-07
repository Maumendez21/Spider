using System;

namespace CredencialSpiderFleet.Models.Main.VehicleList
{
    public class VehicleList
    {

        public string Device { get; set; }
        public string Name { get; set; }
        public DateTime LastDate { get; set; }
        public string CompanyId { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public double Bearing { get; set; }
        public int Status { get; set; }
    }
}