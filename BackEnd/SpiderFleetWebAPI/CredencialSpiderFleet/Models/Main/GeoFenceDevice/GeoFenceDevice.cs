using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.Main.GeoFenceDevice
{
    public class GeoFenceDevice
    {
        public string Id { get; set; }
        //public string Name { get; set; }
        public string IdGeoFence { get; set; }
        public List<string> ListDevice { get; set; }
    }
}