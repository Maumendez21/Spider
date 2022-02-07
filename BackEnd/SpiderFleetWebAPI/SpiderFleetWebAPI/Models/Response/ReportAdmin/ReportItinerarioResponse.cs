using CredencialSpiderFleet.Models.ReportAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.ReportAdmin
{
    public class ReportItinerarioResponse: BasicResponse
    {
        public List<ReportItinerario> itinerarios;

        public ReportItinerarioResponse()
        {
            itinerarios = new List<ReportItinerario>();
        }

    }
}