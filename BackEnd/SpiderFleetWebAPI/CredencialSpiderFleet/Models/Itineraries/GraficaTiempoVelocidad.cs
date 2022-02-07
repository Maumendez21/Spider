using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.Itineraries
{
    public class GraficaTiempoVelocidad
    {
        public string MaximumSpeed { get; set; }
        public List<string> data { get; set; }
        public List<string> label { get; set; }

        public GraficaTiempoVelocidad()
        {
            data = new List<string>();
            label = new List<string>();
        }
    }
}