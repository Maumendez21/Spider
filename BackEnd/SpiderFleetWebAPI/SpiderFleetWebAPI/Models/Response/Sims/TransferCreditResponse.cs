using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Sims
{
    public class TransferCreditResponse : BasicResponse
    {
        public List<CredencialSpiderFleet.Models.Sims.Credit.Sbalance> listBalance { get; set; }

        public TransferCreditResponse()
        {
            listBalance = new List<CredencialSpiderFleet.Models.Sims.Credit.Sbalance>();
        }
    }
}