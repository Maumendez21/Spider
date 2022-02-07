using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.AddtionalVehicleData
{
    public class AddtionalVehicleData
    {
        public string Device { get; set; }
        public string IdMarca { get; set; }
        public string IdModelo { get; set; }
        public string IdVersion { get; set; }
        public string VIN { get; set; }
        public string Placas { get; set; }
        public string Poliza { get; set; }
        public int IdTipoVehiculo { get; set; }
    }
}