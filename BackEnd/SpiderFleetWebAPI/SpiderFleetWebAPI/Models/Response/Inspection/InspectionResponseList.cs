using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Inspection
{
    public class InspectionResponseList : BasicResponse
    {

            public List<CredencialSpiderFleet.Models.Inspection.InspectionList> InspectionList { set; get; }

            public InspectionResponseList()
            {
                InspectionList = new List<CredencialSpiderFleet.Models.Inspection.InspectionList>();
            }
        
    }
}