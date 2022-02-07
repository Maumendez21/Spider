using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Bot
{
    public class TotalResponse: BasicResponse
    {
        public string Total { get; set; }

        public TotalResponse()
        {
            Total = string.Empty;
        }
    }
}