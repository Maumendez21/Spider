using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.ReportAdmin
{
    public class BusinessReportResponse : BasicResponse
    {
        public HttpResponseMessage reporte { get; set; }
      
        public BusinessReportResponse()
        {
            reporte = new HttpResponseMessage();
        }
    }
}