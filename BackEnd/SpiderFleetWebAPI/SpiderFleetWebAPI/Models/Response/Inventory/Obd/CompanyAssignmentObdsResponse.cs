using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Inventory.Obd
{
    public class CompanyAssignmentObdsResponse : BasicResponse
    {
        public List<string> listObds { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public CompanyAssignmentObdsResponse()
        {
            listObds = new List<string>();
        }
    }
}