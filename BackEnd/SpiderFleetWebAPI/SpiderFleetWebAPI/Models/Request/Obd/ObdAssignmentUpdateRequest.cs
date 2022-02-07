using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Request.Obd
{
    public class ObdAssignmentUpdateRequest
    {
        public string IdDevice { get; set; }
        public string Name { get; set; }
        public string Hierarchy { get; set; }
        public int Responsable { get; set; }
    }
}