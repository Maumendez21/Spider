using SpiderFleetStripe.Configuration;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Stripe.Checkout
{
    public class CheckoutController : ApiController
    {

        private const string Tag = "Sripe Invoice";
        private const string BasicRoute = "api/";
        private const string ResourceName = "stripe/invoices";
        private ConfigurationStripe keyStripe = new ConfigurationStripe();


        ///// <summary>
        ///// Metodo que da de alta a un usuario en Stripe
        ///// </summary>
        ///// <param name="email"></param>
        ///// <param name="description"></param>
        ///// <param name="name"></param>
        ///// <param name="phone"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route(BasicRoute + ResourceName)]
        //[SwaggerOperation(Tags = new[] { Tag })]
        //[SwaggerResponse(HttpStatusCode.OK, Type = typeof(StripeCustomerRegistryResponse))]
        //public StripeCustomerRegistryResponse Create(
        //    [FromUri(Name = "email")] string email,
        //    [FromUri(Name = "description")] string description,
        //    [FromUri(Name = "name")] string name,
        //    [FromUri(Name = "phone")] string phone)
        //{
        //    StripeCustomerRegistryResponse response = new StripeCustomerRegistryResponse();

        //    try
        //    {
        //        try
        //        {
        //            response.customer = (new CustomerStripe()).Cretae(keyStripe.skKey, email, description, name, phone);
        //            response.success = true;
        //        }
        //        catch (Exception ex)
        //        {
        //            response.success = false;
        //            response.messages.Add(ex.Message);
        //            return response;
        //        }

        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        response.success = false;
        //        response.messages.Add(ex.Message);
        //        return response;
        //    }
        //}

    }
}
