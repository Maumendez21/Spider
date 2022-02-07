using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Inventory.Obd
{
    public class ListErrorsObdsResponse : BasicResponse
    {
        //public List<CredencialSpiderFleet.Models.Inventory.Obd.CompanyAssignmentObds> listObds { get; set; }
        public Dictionary<string, string> listError { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ListErrorsObdsResponse()
        {
            //listObds = new List<CredencialSpiderFleet.Models.Inventory.Obd.CompanyAssignmentObds>();
            listError = new Dictionary<string, string>();
        }
    }
}