using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Request.Obd
{
    public class SubCompanyAssignmentObdRequest
    {
        public string IdSubCompany { get; set; }
        public List<string> AssignmentObds { get; set; }
    }
}