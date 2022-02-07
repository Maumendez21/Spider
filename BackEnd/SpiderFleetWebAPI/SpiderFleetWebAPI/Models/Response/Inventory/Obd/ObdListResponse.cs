using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Inventory.Obd
{
    public class ObdListResponse : BasicResponse
    {
        public List<CredencialSpiderFleet.Models.Obd.Obd> listObds { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ObdListResponse()
        {
            listObds = new List<CredencialSpiderFleet.Models.Obd.Obd>();
        }
    }
}