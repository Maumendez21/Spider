using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Sims
{
    public class SimsResponse : BasicResponse
    {
        public List<CredencialSpiderFleet.Models.Sims.Sims> listSims { get; set; }

        public SimsResponse()
        {
            listSims = new List<CredencialSpiderFleet.Models.Sims.Sims>();
        }
    }
}