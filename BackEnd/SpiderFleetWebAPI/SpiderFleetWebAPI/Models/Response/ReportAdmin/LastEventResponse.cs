using CredencialSpiderFleet.Models.ReportAdmin;
using System.Collections.Generic;

namespace SpiderFleetWebAPI.Models.Response.ReportAdmin
{
    public class LastEventResponse : BasicResponse
    {
        public List<LastEvent> listEvents { get; set; }

        public LastEventResponse()
        {
            listEvents = new List<LastEvent>();
        }
    }
}