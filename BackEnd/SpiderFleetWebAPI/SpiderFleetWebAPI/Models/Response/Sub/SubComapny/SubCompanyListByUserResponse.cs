using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Sub.SubComapny
{
    public class SubCompanyListByUserResponse : BasicResponse
    {
        public List<CredencialSpiderFleet.Models.Sub.SubComapny.SubCompanyListByUser> listSubCompany { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public SubCompanyListByUserResponse()
        {
            listSubCompany = new List<CredencialSpiderFleet.Models.Sub.SubComapny.SubCompanyListByUser>();
        }
    }
}