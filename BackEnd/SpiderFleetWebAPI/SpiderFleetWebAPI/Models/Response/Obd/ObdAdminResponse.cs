using CredencialSpiderFleet.Models.Obd;
using System.Collections.Generic;

namespace SpiderFleetWebAPI.Models.Response.Obd
{
    public class ObdAdminResponse :  BasicResponse
    {
        public List<ObdAdmin> ListObdAdmins { get; set; }

        public ObdAdminResponse()
        {
            ListObdAdmins = new List<ObdAdmin>();
        }
    }
}