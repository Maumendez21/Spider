using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Sub.SubUser
{
    public class SubUserRegistryResponse: BasicResponse
    {
        public CredencialSpiderFleet.Models.Sub.SubUser.SubUserRegistry user { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public SubUserRegistryResponse()
        {
            user = new CredencialSpiderFleet.Models.Sub.SubUser.SubUserRegistry();
        }
    }
}