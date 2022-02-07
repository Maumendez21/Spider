using CredencialSpiderFleet.Models.Responsible;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Responsible
{
    public class ResponsibleRegistryResponse : BasicResponse
    {
        public ResponsibleRegistry registry { get; set; }
        
        public ResponsibleRegistryResponse()
        {
            registry = new ResponsibleRegistry();
        }
    }
}