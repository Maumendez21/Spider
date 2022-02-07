using CredencialSpiderFleet.Models.Mobility.PointInterestAnalysis;
using SpiderFleetWebAPI.Models.Mongo.PointInterest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Mobility.PointInterestAnalysis
{
    public class PointInterestAnalysisResponse : BasicResponse
    {
        public List<PointInterestAnalysisRegistry> ListAnalysis { get; set; }
        public PointInterestData PointInterest { get; set; }

        public PointInterestAnalysisResponse()
        {
            ListAnalysis = new List<PointInterestAnalysisRegistry>();
            PointInterest = new PointInterestData();
        }
    }
}