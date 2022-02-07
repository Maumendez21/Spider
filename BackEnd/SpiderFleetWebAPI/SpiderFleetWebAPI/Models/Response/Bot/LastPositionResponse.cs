using CredencialSpiderFleet.Models.Bot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Bot
{
    public class LastPositionResponse: BasicResponse
    {
        public LastPosition position { get; set; }

        public LastPositionResponse()
        {
            position = new LastPosition();
        }
    }
}