using CredencialSpiderFleet.Models.ReportAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.ReportAdmin
{
    public class ReportItinerarioSpecialResponse : BasicResponse
    {
        public List<ReportItinerarioSpecial> itinerarios;

        public ReportItinerarioSpecialResponse()
        {
            itinerarios = new List<ReportItinerarioSpecial>();
        }

    }
}