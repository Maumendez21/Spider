using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Catalog.SuscriptionsType
{
    public class SuscriptionsTypeListResponse : BasicResponse
    {
        public List<CredencialSpiderFleet.Models.Catalogs.SuscriptionsType.SuscriptionType> listType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public SuscriptionsTypeListResponse()
        {
            listType = new List<CredencialSpiderFleet.Models.Catalogs.SuscriptionsType.SuscriptionType>();
        }
    }
}