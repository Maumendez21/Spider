using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Catalog.SuscriptionsType
{
    public class SuscriptionTypeRegistryResponse: BasicResponse
    {
        public CredencialSpiderFleet.Models.Catalogs.SuscriptionsType.SuscriptionType suscriptionType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public SuscriptionTypeRegistryResponse()
        {
            suscriptionType = new CredencialSpiderFleet.Models.Catalogs.SuscriptionsType.SuscriptionType();
        }
    }
}