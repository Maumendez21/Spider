using CredencialSpiderFleet.Models.Obd;
using System.Collections.Generic;

namespace SpiderFleetWebAPI.Models.Response.Obd
{
    public class ListObdResponse : BasicResponse
    {
        public List<ObdCompany> listObd { get; set; }

        public ListObdResponse()
        {
            listObd = new List<ObdCompany>();
        }
    }
}
