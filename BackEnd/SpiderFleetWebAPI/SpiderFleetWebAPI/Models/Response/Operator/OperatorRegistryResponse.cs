using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Operator
{
    public class OperatorRegistryResponse : BasicResponse
    {
        public CredencialSpiderFleet.Models.Operator.OperatorRegistry operators { get; set; }

        public OperatorRegistryResponse ()
        {
            operators = new CredencialSpiderFleet.Models.Operator.OperatorRegistry();
        }
    }
}