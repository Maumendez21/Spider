using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Company
{
    public class CompanyListLevelOneResponse : BasicResponse
    {
        public List<CredencialSpiderFleet.Models.Company.CompanyList> listCompany { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public CompanyListLevelOneResponse()
        {
            listCompany = new List<CredencialSpiderFleet.Models.Company.CompanyList>();
        }
    }
}