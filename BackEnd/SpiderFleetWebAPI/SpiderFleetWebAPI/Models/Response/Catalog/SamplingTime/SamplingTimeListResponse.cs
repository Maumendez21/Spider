using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Catalog.SamplingTime
{
    public class SamplingTimeListResponse: BasicResponse
    {
        public List<CredencialSpiderFleet.Models.Catalogs.SamplingTime.SamplingTimeRegistry> ListSamTime { get; set; }

        public SamplingTimeListResponse()
        {
            ListSamTime = new List<CredencialSpiderFleet.Models.Catalogs.SamplingTime.SamplingTimeRegistry>();
        }
    }
}