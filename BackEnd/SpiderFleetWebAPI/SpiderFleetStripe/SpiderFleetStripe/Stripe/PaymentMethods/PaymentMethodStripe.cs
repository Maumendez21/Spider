using Stripe;
using System;


namespace SpiderFleetStripe.Stripe.PaymentMethods
{
    public class PaymentMethodStripe
    {
        //https://stripe.com/docs/api/payment_methods/create
        public PaymentMethod Cretae(string key, string idUserStrpe, string number, long expmonth, long expyear, string cvc)
        {
            PaymentMethod method = new PaymentMethod();
            try
            {
                StripeConfiguration.ApiKey = key;

                var options = new PaymentMethodCreateOptions
                {
                    Type = "card",
                    Card = new PaymentMethodCardCreateOptions // PaymentMethodCardOptions
                    {
                        Number = number,// "4242424242424242",
                        ExpMonth = expmonth,// 1,
                        ExpYear = expyear, // 2021,
                        Cvc =  cvc,// "314",
                    },                   
                    //Customer = idUserStrpe,
                };
                var service = new PaymentMethodService();
                method = service.Create(options);

                return method;
            }
            catch (Exception ex)
            {
                return method;
            }
        }

        public int Update(string key, string idUserStrpe, string number, long expmonth, long expyear, string cvc)
        {
            int respuesta = 0;
            try
            {
                StripeConfiguration.ApiKey = key;

                var options = new PaymentMethodCreateOptions
                {
                    Type = "card",
                    Card = new PaymentMethodCardCreateOptions // PaymentMethodCardOptions
                    {
                        Number = number,// "4242424242424242",
                        ExpMonth = expmonth,// 1,
                        ExpYear = expyear, // 2021,
                        Cvc = cvc,// "314",
                    },
                    Customer = idUserStrpe,
                };
                var service = new PaymentMethodService();
                service.Create(options);

                respuesta = 1;
                return respuesta;
            }
            catch (Exception ex)
            {
                return respuesta;
            }
        }

        public PaymentMethod ReadId(string key, string idPayMethod)
        {
            PaymentMethod payment = new PaymentMethod();
            try
            {
                StripeConfiguration.ApiKey = key;

                var service = new PaymentMethodService();
                payment = service.Get(idPayMethod);// "pm_1EUoZ0BHWpkjRuwwTXf6RiHV");

                return payment;
            }
            catch (Exception ex)
            {
                return payment;
            }
        }

        public object Read(string key, string idUserStrpe)
        {
            object listPayment = new object();

            try
            {
                StripeConfiguration.ApiKey = key;

                var options = new PaymentMethodListOptions
                {
                    Customer = idUserStrpe,//  "cus_GbMIh1YjWNpngc",
                    Type = "card",
                };
                var service = new PaymentMethodService();
                listPayment = service.List(options);
                

                return listPayment;
            }
            catch (Exception ex)
            {
                return listPayment;
            }
        }

        public int AttachPaymentMethodCustomer(string key, string idUserStrpe, string idMethod)
        {
            int respuesta = 0;
            try
            {
                StripeConfiguration.ApiKey = key;

                var options = new PaymentMethodAttachOptions
                {
                    Customer = idUserStrpe,// "cus_GbMIh1YjWNpngc",
                };
                var service = new PaymentMethodService();
                service.Attach(idMethod, options); //service.Attach("pm_123456789", options);

                respuesta = 1;
                return respuesta;
            }
            catch (Exception ex)
            {
                return respuesta;
            }
        }

        public int DetachPaymentMethodCustomer(string key, string idMethod)
        {
            int respuesta = 0;
            try
            {
                StripeConfiguration.ApiKey = key;

                var service = new PaymentMethodService();
                service.Detach(idMethod);// "pm_123456789");

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