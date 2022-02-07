using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Request.Admin
{
    public class CompanyAdminRequest
    {
        public string Company { get; set; }
        public string Device { get; set; }
    }
}