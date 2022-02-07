using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.Obd
{
    public class ObdAssignmentUpdate
    {
        public string IdDevice { get; set; }
        public string Name { get; set; }
        public string Hierarchy { get; set; }
        public int Responsable { get; set; }
    }
}