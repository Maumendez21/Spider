using System.Collections.Generic;

namespace SpiderFleetWebAPI.Models.Response.Obd
{
    public class ObdListResponse : BasicResponse
    {
        public List<CredencialSpiderFleet.Models.Obd.ObdRegistry> listObd { get; set; }

        public ObdListResponse()
        {
            listObd = new List<CredencialSpiderFleet.Models.Obd.ObdRegistry>();
        }
    }
}