using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Sims
{
    public class ReportCreditSimsResponse : BasicResponse
    {
        public List<CredencialSpiderFleet.Models.Sims.ReportSims> listReportSims { get; set; }

        public ReportCreditSimsResponse()
        {
            listReportSims = new List<CredencialSpiderFleet.Models.Sims.ReportSims>();
        }
    }
}