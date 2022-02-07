using CredencialSpiderFleet.Models.ReportAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.ReportAdmin
{
    public class ReportLastStatusDeviceResponse: BasicResponse
    {
        public ReportLastStatusDevice statusDevice;

        public List<ReportLastStatusAlarms> listAlarms;

        public ReportLastStatusDeviceResponse()
        {
            statusDevice = new ReportLastStatusDevice();
            listAlarms = new List<ReportLastStatusAlarms>();
        }
    }
}