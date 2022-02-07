using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Inspection
{
    public class headersandTemplatesResponse : BasicResponse
    {
        public List<CredencialSpiderFleet.Models.Inspection.Headerandtemplate> PlantillaHeader { set; get; }

        public headersandTemplatesResponse()
        {
            PlantillaHeader = new List<CredencialSpiderFleet.Models.Inspection.Headerandtemplate>();
        }
    }
}