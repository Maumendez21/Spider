using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Company
{
    public class CompanyRegistryResponse: BasicResponse
    {
        public CredencialSpiderFleet.Models.Company.Company company { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public CompanyRegistryResponse()
        {
            company = new CredencialSpiderFleet.Models.Company.Company();
        }
    }
}