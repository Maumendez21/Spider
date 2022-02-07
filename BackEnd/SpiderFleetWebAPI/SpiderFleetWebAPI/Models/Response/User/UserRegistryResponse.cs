using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.User
{
    public class UserRegistryResponse :  BasicResponse
    {
        public CredencialSpiderFleet.Models.User.UserRegistry user { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public UserRegistryResponse()
        {
            user = new CredencialSpiderFleet.Models.User.UserRegistry();
        }
    }
}