using SpiderFleetWebAPI.Models.Mongo.PointInterest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.PointsInterest
{
    public class PointInerestListResponse : BasicResponse
    {
        public List<PointInterestData> ListPointInterest { get; set; }

        public PointInerestListResponse()
        {
            ListPointInterest = new List<PointInterestData>();
        }
    }
}