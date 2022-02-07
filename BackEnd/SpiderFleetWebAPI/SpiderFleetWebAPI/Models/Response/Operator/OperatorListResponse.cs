using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Operator
{
    public class OperatorListResponse : BasicResponse
    {
        public List<CredencialSpiderFleet.Models.Operator.OperatorRegistry> listOperator { get; set; }

        public OperatorListResponse()
        {
            listOperator = new List<CredencialSpiderFleet.Models.Operator.OperatorRegistry>();
        }
    }
}