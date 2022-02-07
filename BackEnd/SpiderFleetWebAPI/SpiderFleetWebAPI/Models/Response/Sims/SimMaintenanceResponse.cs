using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Sims
{
    public class SimMaintenanceResponse : BasicResponse
    {
        public List<CredencialSpiderFleet.Models.Sims.Credit.Sbalance> listBalance { get; set; }

        public SimMaintenanceResponse()
        {
            listBalance = new List<CredencialSpiderFleet.Models.Sims.Credit.Sbalance>();
        }
    }
}