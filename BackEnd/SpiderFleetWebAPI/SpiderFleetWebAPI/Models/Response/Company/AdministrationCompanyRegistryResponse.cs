using CredencialSpiderFleet.Models.Company;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Company
{
    public class AdministrationCompanyRegistryResponse: BasicResponse
    {
        public AdministrationCompanyRegistry registry { get; set; }

        public AdministrationCompanyRegistryResponse()

        {
            registry = new AdministrationCompanyRegistry();
        }
    }
}