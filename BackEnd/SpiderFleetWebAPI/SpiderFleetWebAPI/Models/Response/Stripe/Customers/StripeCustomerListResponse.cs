using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Stripe.Customers
{
    public class StripeCustomerListResponse : BasicResponse
    {

        public object customer { get; set; }

        public StripeCustomerListResponse()
        {
            customer = new object();
        }
    }
}