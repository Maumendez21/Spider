using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Sub.SubComapny
{
    public class ListErrorSubCompanyResponse : BasicResponse
    {
        public Dictionary<string, string> listError { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ListErrorSubCompanyResponse()
        {
            listError = new Dictionary<string, string>();
        }
    }
}