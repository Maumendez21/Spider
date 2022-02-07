using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.RouteDiary
{
    public class RouteDiaryResgitrys
    {
        public int IdStart { get; set; }
        public DateTime StartDate { get; set; }
        public int IdEnd { get; set; }
        public DateTime EndDate { get; set; }
        public string Notes { get; set; }
        public string Device { get; set; }
        public string Vehicle { get; set; }
        public string Route { get; set; }
        public string Name { get; set; }
    }
}