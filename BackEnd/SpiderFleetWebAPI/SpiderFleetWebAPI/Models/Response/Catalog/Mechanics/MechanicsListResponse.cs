using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Catalog.Mechanics
{
    public class MechanicsListResponse : BasicResponse
    {
        public List<CredencialSpiderFleet.Models.Catalogs.Mechanics.Mechanics> ListMechanics { set; get; }

        public MechanicsListResponse()
        {
            ListMechanics = new List<CredencialSpiderFleet.Models.Catalogs.Mechanics.Mechanics>();
        }
    }
}