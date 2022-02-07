using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Company
{
    public class CompanyListResponse : BasicResponse
    {
        public List<CredencialSpiderFleet.Models.Company.Company> listCompany { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public CompanyListResponse()
        {
            listCompany = new List<CredencialSpiderFleet.Models.Company.Company>();
        }
    }
}