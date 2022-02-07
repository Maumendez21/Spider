using CredencialSpiderFleet.Models.ReportAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.ReportAdmin
{
    public class ReportDeviceLastDataResponse: BasicResponse
    {

        public List<ReportDeviceLastData> listLogin;
        public List<ReportDeviceLastData> listAlarm;     
        
        public ReportDeviceLastDataResponse ()
        {
            listLogin = new List<ReportDeviceLastData>();
            listAlarm = new List<ReportDeviceLastData>();
        }
    }
}