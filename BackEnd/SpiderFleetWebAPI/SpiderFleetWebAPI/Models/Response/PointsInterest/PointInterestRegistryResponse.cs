using SpiderFleetWebAPI.Models.Mongo.PointInterest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.PointsInterest
{
    public class PointInterestRegistryResponse : BasicResponse
    {
        public PointInterestData PointInterest { get; set; }

        public PointInterestRegistryResponse()
        {
            PointInterest = new PointInterestData();
        }

    }
}