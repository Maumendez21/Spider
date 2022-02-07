using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Catalog.Version
{
    public class VersionListResponse : BasicResponse
    {
        public List<CredencialSpiderFleet.Models.Catalogs.Version.Version> ListVersions { get; set; }

        public VersionListResponse()
        {
            ListVersions = new List<CredencialSpiderFleet.Models.Catalogs.Version.Version>();
        }
    }
}