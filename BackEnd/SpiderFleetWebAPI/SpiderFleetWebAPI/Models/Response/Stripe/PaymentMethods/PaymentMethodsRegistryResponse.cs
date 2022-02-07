using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Stripe.PaymentMethods
{
    public class PaymentMethodsRegistryResponse : BasicResponse
    {
        public object paymentmethods { get; set; }

        public PaymentMethodsRegistryResponse()
        {
            paymentmethods = new object();
        }
    }
}