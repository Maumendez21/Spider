using CredencialSpiderFleet.Models.Logical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Logical
{
    public class LogicalResponse : BasicResponse
    {
        public List<RawData> ListRawData { get; set; }

        public LogicalResponse ()
        {
            ListRawData = new List<RawData>();
        }
    }
}