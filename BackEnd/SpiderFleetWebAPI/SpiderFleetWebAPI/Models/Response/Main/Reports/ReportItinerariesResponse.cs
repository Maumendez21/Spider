using CredencialSpiderFleet.Models.Main.Reports;
using System.Collections.Generic;

namespace SpiderFleetWebAPI.Models.Response.Main.Reports
{
    public class ReportItinerariesResponse : BasicResponse
    {
        public List<ReportHeaderTrip> listHeader { get; set; }
        public List<ReportTrip> listTrip { get; set; }

        public ReportItinerariesResponse()
        {
            listHeader = new List<ReportHeaderTrip>();
            listTrip = new List<ReportTrip>();
        }
    }
}