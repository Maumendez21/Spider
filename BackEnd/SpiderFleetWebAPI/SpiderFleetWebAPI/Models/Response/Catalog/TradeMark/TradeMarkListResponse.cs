using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Catalog.TradeMark
{
    public class TradeMarkListResponse: BasicResponse
    {
        public List<CredencialSpiderFleet.Models.Catalogs.TradeMark.TradeMark> ListMarks { get; set; }

        public TradeMarkListResponse()
        {
            ListMarks = new List<CredencialSpiderFleet.Models.Catalogs.TradeMark.TradeMark>();
        }
    }
}