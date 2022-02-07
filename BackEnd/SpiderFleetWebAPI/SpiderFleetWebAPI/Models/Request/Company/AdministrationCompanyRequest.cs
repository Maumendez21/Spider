using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Request.Company
{
    public class AdministrationCompanyRequest
    {
        public string Node { get; set; }
        public int Active { get; set; }
    }
}