using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetWebAPI.Models.Response.Stripe.PaymentMethods
{
    public class PaymentMethodsListResponse : BasicResponse
    {
        public object paymentmethodList { get; set; }

        public PaymentMethodsListResponse()
        {
            paymentmethodList = new object();
        }
    }
}