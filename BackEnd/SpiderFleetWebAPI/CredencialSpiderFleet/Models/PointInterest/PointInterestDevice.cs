using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.PointInterest
{
    public class PointInterestDevice
    {
        public string Id { get; set; }
        public string IdPointInterest { get; set; }
        public List<string> ListDevice { get; set; }
    }
}