using CredencialSpiderFleet.Models.PointInterest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.PointsInterest
{
    public class PointInterestListRegistryResponse :  BasicResponse
    {
        public List<PointInterestRegistry> ListPoints { get; set; }

        public PointInterestListRegistryResponse()
        {
            ListPoints = new List<PointInterestRegistry>();
        }
    }
}