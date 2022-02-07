using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Credentials
{
    public class LoginCredentialsResponse : BasicResponse
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public string refresh_token { get; set; }
        public string name { get; set; }
        public string role { get; set; }
        public string idu { get; set; }
        public string image { get; set; }
        public string spider { get; set; }
        public List<string> listPermissions { get; set; }

        public LoginCredentialsResponse()
        {
            this.access_token = "";
            this.expires_in = 0;
            this.refresh_token = "";
            name = string.Empty;
            role = string.Empty;
            idu = string.Empty;
            image = string.Empty;
            spider = string.Empty;
            listPermissions = new List<string>();
        }
    }
}