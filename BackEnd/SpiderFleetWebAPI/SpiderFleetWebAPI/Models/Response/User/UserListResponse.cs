using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.User
{
    public class UserListResponse: BasicResponse
    {
        public List<CredencialSpiderFleet.Models.User.UserRegistry> listUsers { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public UserListResponse()
        {
            listUsers = new List<CredencialSpiderFleet.Models.User.UserRegistry>();
        }
    }
}