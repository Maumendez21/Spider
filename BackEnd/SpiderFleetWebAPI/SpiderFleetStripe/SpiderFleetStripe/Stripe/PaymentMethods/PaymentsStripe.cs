using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpiderFleetStripe.Stripe.PaymentMethods
{
    public class PaymentsStripe
    {

        public int Cretae(string key, long amount, string currency, string idUsrStripe, string paymentmethod)
        {
            int respuesta = 0;
            try
            {
                StripeConfiguration.ApiKey = key;

                var service = new PaymentIntentService();
                var options = new PaymentIntentCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card" },
                    Amount = amount,// 1099,
                    Currency = currency,// "mxn",
                    Customer = idUsrStripe,// "cus_GbiSKSF5wjQ6tM",
                    PaymentMethod = paymentmethod,// "card_1G4V20GI3wXPbnaM5euXOG4b"
                };
                service.Create(options);

                respuesta = 1;
                return respuesta;
            }
            catch (Exception ex)
            {
                return respuesta;
            }
        }
    }
}