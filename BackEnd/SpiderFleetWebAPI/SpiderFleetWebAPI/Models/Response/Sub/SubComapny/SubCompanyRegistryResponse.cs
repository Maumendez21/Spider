using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Sub.SubComapny
{
    public class SubCompanyRegistryResponse: BasicResponse
    {
        public CredencialSpiderFleet.Models.Sub.SubComapny.SubCompanyData subCompany { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public SubCompanyRegistryResponse()
        {
            subCompany = new CredencialSpiderFleet.Models.Sub.SubComapny.SubCompanyData();
        }
    }
}