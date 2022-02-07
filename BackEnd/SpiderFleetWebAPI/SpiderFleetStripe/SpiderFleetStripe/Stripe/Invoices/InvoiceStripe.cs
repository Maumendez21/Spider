using Stripe;
using System;

namespace SpiderFleetStripe.Stripe.Invoices
{
    public class InvoiceStripe
    {

        public int Create(string key, string idUserStripe, long amount, string currency, string description)
        {
            int respuesta = 0;
            try
            {
                StripeConfiguration.ApiKey = key;

                var options = new InvoiceItemCreateOptions
                {
                    Customer = idUserStripe,// "cus_4fdAW5ftNQow1a",
                    Amount = amount,// 2500,
                    Currency = currency,// "usd",
                    Description = description,// "One-time setup fee",
                };
                var service = new InvoiceItemService();
                service.Create(options);
                var invoiceOptions = new InvoiceCreateOptions
                {
                    Customer = idUserStripe,// "cus_4fdAW5ftNQow1a",
                    AutoAdvance = true,  //This means Stripe automatically finalizes the invoice after a few hours, making the invoice ready for payment
                };
                var invoiceService = new InvoiceService();
                invoiceService.Create(invoiceOptions);
                return respuesta;
            }
            catch (Exception ex)
            {
                return respuesta;
            }
            
        }
    }
}