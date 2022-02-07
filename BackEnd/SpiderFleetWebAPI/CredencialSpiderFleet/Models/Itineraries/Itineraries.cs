using System;
using System.Collections.Generic;

namespace CredencialSpiderFleet.Models.Itineraries
{
    public class Itineraries
    {
        public string Device { get; set; }
        public DateTime StartDate { get; set; }
        public string StartHour { get; set; }
        public DateTime EndDate { get; set; }
        public string EndHour { get; set; }
        public string VehicleName { get; set; }
        public string DriverData { get; set; }
        public string Image { get; set; }
        public string ODO { get; set; }
        public string Fuel { get; set; }
        public string Score { get; set; }
        public string Time { get; set; }

        public string TravelDate { get; set; }
        

        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Version { get; set; }
        public string TipoVehiculo { get; set; }
        public string VIN { get; set; }
        public string Placas { get; set; }
        public string Poliza { get; set; }

        public string Label { get; set; }
        public string Batery { get; set; }
        public int TypeDevice { get; set; }
        public int EngineStop { get; set; }
        public string Hierarchy { get; set; }

        public double totalDistanciaDouble { get; set; }
    }
}