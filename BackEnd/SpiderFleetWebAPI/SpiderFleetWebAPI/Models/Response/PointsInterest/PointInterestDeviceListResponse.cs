using SpiderFleetWebAPI.Models.Mongo.PointInterest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.PointsInterest
{
    public class PointInterestDeviceListResponse :  BasicResponse
    {
        public PointInterestList PointInterest { get; set; }

        public PointInterestDeviceListResponse()
        {
            PointInterest = new PointInterestList();
        }
    }
}