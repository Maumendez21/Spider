using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Inspection
{
    public class InspectionResponse : BasicResponse
    {
        public List<CredencialSpiderFleet.Models.Inspection.InspectionResults> listinspection { set; get; }

        public string folio { set; get; }

        public InspectionResponse()
        {
            listinspection = new List<CredencialSpiderFleet.Models.Inspection.InspectionResults>();

            folio = string.Empty;
        }
    }
}