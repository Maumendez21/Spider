using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Company
{
    public class AssignmentObdsResponse : BasicResponse
    {
        public List<CredencialSpiderFleet.Models.Company.AssignmentObds> listObds { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public AssignmentObdsResponse()
        {
            listObds = new List<CredencialSpiderFleet.Models.Company.AssignmentObds>();
        }
    }
}