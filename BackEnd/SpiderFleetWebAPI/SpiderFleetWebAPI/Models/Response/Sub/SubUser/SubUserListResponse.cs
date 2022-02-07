using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Sub.SubUser
{
    public class SubUserListResponse: BasicResponse
    {
        public List<CredencialSpiderFleet.Models.Sub.SubUser.SubUserRegistry> listUsers { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public SubUserListResponse()
        {
            listUsers = new List<CredencialSpiderFleet.Models.Sub.SubUser.SubUserRegistry>();
        }
    }
}