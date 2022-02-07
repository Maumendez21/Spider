using SpiderFleetStripe.Configuration;
using SpiderFleetStripe.Stripe.Invoices;
using SpiderFleetWebAPI.Models.Response.Stripe.Invoices;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Stripe.Invoices
{
    public class InvoicesController : ApiController
    {

        private const string Tag = "Sripe Invoives";
        private const string BasicRoute = "api/";
        private const string ResourceName = "stripe/invoices";
        private ConfigurationStripe keyStripe = new ConfigurationStripe();


        /// <summary>
        /// Metodo que da de alta a un usuario en Stripe
        /// </summary>
        /// <param name="email"></param>
        /// <param name="description"></param>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        [HttpPost]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(StripeInvoiceResponse))]
        public StripeInvoiceResponse Create(
            [FromUri(Name = "idUserStripe")] string idUserStripe,
            [FromUri(Name = "currency")] string currency,
            [FromUri(Name = "description")] string description,
            [FromUri(Name = "amount")] long amount)
        {
            StripeInvoiceResponse response = new StripeInvoiceResponse();

            try
            {
                try
                {
                    int respuesta = (new InvoiceStripe()).Create(keyStripe.skKey, idUserStripe, amount, currency, description);
                    response.success = true;
                }
                catch (Exception ex)
                {
                    response.success = false;
                    response.messages.Add(ex.Message);
                    return response;
                }

                return response;
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }
        }


    }
}
