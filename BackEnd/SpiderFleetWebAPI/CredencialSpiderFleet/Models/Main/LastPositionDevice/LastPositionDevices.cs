
using System;

namespace CredencialSpiderFleet.Models.Main.LastPositionDevice
{
    public class LastPositionDevices
    {
        public string dispositivo { get; set; }
        public string nombre { get; set; }
        public DateTime fecha { get; set; }
        public string latitud { get; set; }
        public string longitud { get; set; }
        public string empresa { get; set; }
        public string velocidad { get; set; }
        public string direccion { get; set; }
        public string odo { get; set; }
        public bool estado { get; set; }
        public string operador { get; set; }
    }
}