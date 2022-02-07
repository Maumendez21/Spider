using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Request.Inventory.Obd
{
    public class CompanyAssignmentObdsRequest
    {
        public int IdCompany { get; set; }
        public List<string> AssignmentObds { get; set; }
    }
}