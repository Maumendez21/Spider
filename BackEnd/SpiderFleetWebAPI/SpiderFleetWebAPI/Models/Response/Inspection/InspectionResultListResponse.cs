using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Inspection
{
    public class InspectionResultListResponse : BasicResponse
    {


        public List<CredencialSpiderFleet.Models.Inspection.ListResultHeaderInspection> results { set; get; }

        public InspectionResultListResponse()
        {
            results = new List<CredencialSpiderFleet.Models.Inspection.ListResultHeaderInspection>();
        }

    }
}