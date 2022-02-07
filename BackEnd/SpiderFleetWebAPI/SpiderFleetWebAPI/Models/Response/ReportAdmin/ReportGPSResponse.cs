using CredencialSpiderFleet.Models.ReportAdmin;
using System.Collections.Generic;

namespace SpiderFleetWebAPI.Models.Response.ReportAdmin
{
    public class ReportGPSResponse :  BasicResponse
    {
        public List<ReportGPS> listGps = new List<ReportGPS>();

        public ReportGPSResponse()
        {
            listGps = new List<ReportGPS>();
        }
    }
}