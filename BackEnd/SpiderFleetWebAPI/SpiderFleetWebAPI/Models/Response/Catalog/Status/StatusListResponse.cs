using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Catalog.Status
{
    public class StatusListResponse : BasicResponse
    {
        public List<CredencialSpiderFleet.Models.Catalogs.Status.Status> listStatus { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public StatusListResponse()
        {
            listStatus = new List<CredencialSpiderFleet.Models.Catalogs.Status.Status>();
        }
    }
}