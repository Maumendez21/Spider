using CredencialSpiderFleet.Models.Company;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Company
{
    public class AdministrationCompanyListResponse: BasicResponse
    {
        public List<AdministrationCompanyRegistry> ListCompany { get; set; }

        public AdministrationCompanyListResponse()
        {
            ListCompany = new List<AdministrationCompanyRegistry>();
        }
    }
}