using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.Inventory.Obd
{
    public class ObdInfoUser
    {
        public string IdDevice { get; set; }
        public string Name { get; set; }
        public string LicensePlate { get; set; }
        public int IdImagen { get; set; }
        public string hierarchy { get; set; }

        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Event { get; set; }
        public DateTime Date { get; set; }
        public int Status { get; set; }


    }
}