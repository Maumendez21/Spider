using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CredencialSpiderFleet.Models.Catalogs.TypeService
{
    public class TypeService
    {
        public int IdTypeService { get; set; }
        public string Description { get; set; }
        public string EstimatedTime { get; set; }
        public double EstimatedCost { get; set; }

    }
}