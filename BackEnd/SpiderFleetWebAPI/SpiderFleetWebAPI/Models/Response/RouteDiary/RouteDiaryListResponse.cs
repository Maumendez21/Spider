using CredencialSpiderFleet.Models.RouteDiary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.RouteDiary
{
    public class RouteDiaryListResponse : BasicResponse
    {
        public List<RouteDiaryResgitrys> ListEvents { get; set; }

        public RouteDiaryListResponse()
        {
            ListEvents = new List<RouteDiaryResgitrys>();
        }
    }
}