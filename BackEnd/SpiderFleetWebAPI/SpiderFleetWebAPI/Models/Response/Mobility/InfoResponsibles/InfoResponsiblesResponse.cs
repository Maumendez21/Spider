using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Mobility.InfoResponsibles
{
    public class InfoResponsiblesResponse : BasicResponse
    {
        public List<CredencialSpiderFleet.Models.Mobility.InfoResponsibles.InfoResponsibles> ListResponsibles { get; set; }

        public InfoResponsiblesResponse()
        {
            ListResponsibles = new List<CredencialSpiderFleet.Models.Mobility.InfoResponsibles.InfoResponsibles>();
        }
    }
}