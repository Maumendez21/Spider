using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.DashBoard
{
    public class Graficas
    {
        public List<string> data { get; set; }
        public List<string> label { get; set; }

        public Graficas()
        {
            data = new List<string>();
            label = new List<string>();
        }
    }
}