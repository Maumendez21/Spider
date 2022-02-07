using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Obd
{
    public class ListErrorObdResponse : BasicResponse
    {
        public Dictionary<string, string> listError { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ListErrorObdResponse()
        {
            listError = new Dictionary<string, string>();
        }
    }
}