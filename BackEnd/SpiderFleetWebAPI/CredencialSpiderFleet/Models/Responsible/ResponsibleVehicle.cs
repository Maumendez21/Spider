using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.Responsible
{
    public class ResponsibleVehicle
    {
        public string Device { get; set; }
        public string Vehicle { get; set; }
        public string Responsible { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Area { get; set; }
        public int IdDongle { get; set; }
        public string Dongle { get; set; }
        public int Motor { get; set; }
        public string Hierarchy { get; set; }
    }
}