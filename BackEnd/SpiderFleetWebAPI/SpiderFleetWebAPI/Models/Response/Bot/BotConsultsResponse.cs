using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Bot
{
    public class BotConsultsResponse: BasicResponse
    {
        public int Data { get; set; }

        public BotConsultsResponse()
        {
            Data = 0;
        }
    }
}