using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Inventory.Obd
{
    public class ObdInfoUserListResponse : BasicResponse
    {
        public List<CredencialSpiderFleet.Models.Inventory.Obd.ObdInfoUser> listObdInfo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ObdInfoUserListResponse()
        {
            listObdInfo = new List<CredencialSpiderFleet.Models.Inventory.Obd.ObdInfoUser>();
        }
    }
}