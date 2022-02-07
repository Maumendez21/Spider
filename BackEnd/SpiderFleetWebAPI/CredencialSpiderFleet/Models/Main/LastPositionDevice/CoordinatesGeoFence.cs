using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.Main.LastPositionDevice
{
    public class CoordinatesGeoFence
    {
        public List<List<double>> Coordinates { get; set; }
        public string name { get; set; }

        public CoordinatesGeoFence()
        {
            name = string.Empty;
            Coordinates = new List<List<double>>();
        }
    }
}