using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Stripe.Customers
{
    public class StripeCustomerRegistryResponse : BasicResponse
    {

        public Object customer { get; set; }

        public StripeCustomerRegistryResponse()
        {
            customer = new object();
        }
    }
}