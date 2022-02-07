using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Details
{
    public class DetailsRegistryResponse: BasicResponse
    {
        public CredencialSpiderFleet.Models.Details.DetailsRegistry Registry { get; set; }

        public DetailsRegistryResponse()
        {
            Registry = new CredencialSpiderFleet.Models.Details.DetailsRegistry();
        }
    }
}