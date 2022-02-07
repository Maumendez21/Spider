using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Details
{
    public class DetailsListResponse : BasicResponse
    {
        public List<CredencialSpiderFleet.Models.Details.DetailsRegistry> ListDetails { get; set; }

        public DetailsListResponse()
        {
            ListDetails = new List<CredencialSpiderFleet.Models.Details.DetailsRegistry>();
        }
    }
}