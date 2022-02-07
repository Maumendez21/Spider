using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Catalog.CommunicationMethods
{
    public class CommunicationMethodsListResponse : BasicResponse
    {

        public List<CredencialSpiderFleet.Models.Catalogs.CommunicationMethods.CommunicationMethodsRegistry> ListComMethods { get; set; }

        public CommunicationMethodsListResponse()
        {
            ListComMethods = new List<CredencialSpiderFleet.Models.Catalogs.CommunicationMethods.CommunicationMethodsRegistry>();
        }
    }
}