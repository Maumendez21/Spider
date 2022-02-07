using System.Collections.Generic;

namespace SpiderFleetWebAPI.Models.Response.Sub.SubComapny
{
    public class SubCompanyListResponse : BasicResponse
    {
        public List<CredencialSpiderFleet.Models.Sub.SubComapny.SubCompanyData> listSubCompany { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public SubCompanyListResponse()
        {
            listSubCompany = new List<CredencialSpiderFleet.Models.Sub.SubComapny.SubCompanyData>();
        }
    }
}