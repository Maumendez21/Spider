using CredencialSpiderFleet.Models.Main.Reports;
using CredencialSpiderFleet.Models.Main.TraceTrip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Main.Reports
{
    public class ReportsResponse : BasicResponse
    {
        public List<ReportConduct> listPoints { get; set; }

        public ReportsResponse()
        {
            listPoints = new List<ReportConduct>();
        }
    }
}