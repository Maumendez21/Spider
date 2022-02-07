using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.AddtionalVehicleData
{
    public class AddtionalVehicleDataRegistry
    {
        public string Device { get; set; }
        public string Name { get; set; }
        public string IdMarca { get; set; }
        public string Marca { get; set; }
        public string IdModelo { get; set; }
        public string Modelo { get; set; }
        public string IdVersion { get; set; }
        public string Version { get; set; }
        public string VIN { get; set; }
        public string Placas { get; set; }
        public string Poliza { get; set; }
        public int IdTipoVehiculo { get; set; }
        public string TipoVehiculo { get; set; }
    }
}