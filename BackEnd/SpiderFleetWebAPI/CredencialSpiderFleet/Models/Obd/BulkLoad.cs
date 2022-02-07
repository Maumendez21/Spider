using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.Obd
{
    public class BulkLoad
    {
        public string Device { get; set; }
	    public string Node { get; set; }
        public string IdSpider { get; set; }
        public string Name { get; set; }
        public int Dongle { get; set; }
        public string Empresa { get; set; }
        public string Sim { get; set; }
    }
}