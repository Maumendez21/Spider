using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Request.Catalog.TypeService
{
    public class TypeServiceRequest
    {
        public string Description { get; set; }
        public string EstimatedTime { get; set; }
        public double EstimatedCost { get; set; }

    }
}