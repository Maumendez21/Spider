using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.EngineStop
{
    public class EngineStopNumberPagesResponse :  BasicResponse
    {

        public int NumberPages { get; set; }

        public EngineStopNumberPagesResponse()
        {
            NumberPages = 0;
        }

    }
}