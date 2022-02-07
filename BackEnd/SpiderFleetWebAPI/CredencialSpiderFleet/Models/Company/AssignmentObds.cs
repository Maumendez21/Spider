using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.Company
{
    public class AssignmentObds
    {
        public string IdDevice { get; set; }
        public string Label { get; set; }
        public int IdSubCompany { get; set; }
        public string SubCompany { get; set; }
        public string Jerarquia { get; set; }
        public int IdTypeDevice { get; set; }
        public string Description { get; set; }
    }
}