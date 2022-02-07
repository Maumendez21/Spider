using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.SnappedPoints
{
    public class SnappedPoint
    {
        public Location location { get; set; }
        public int originalIndex { get; set; }
        public string placeId { get; set; }
    }
}