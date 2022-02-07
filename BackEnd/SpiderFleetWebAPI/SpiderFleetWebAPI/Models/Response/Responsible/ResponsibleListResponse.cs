using CredencialSpiderFleet.Models.Responsible;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Responsible
{
    public class ResponsibleListResponse: BasicResponse
    {
        public List<ResponsibleRegistry> listResponsible { get; set; }

        public ResponsibleListResponse()
        {
            listResponsible = new List<ResponsibleRegistry>();
        }
    }
}