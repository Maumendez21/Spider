using SpiderFleetStripe.Configuration;
using SpiderFleetStripe.Stripe.PaymentMethods;
using SpiderFleetWebAPI.Models.Response.Stripe.PaymentMethods;
using Stripe;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Stripe.PaymentMethods
{
    public class PaymentMethodsController : ApiController
    {

        private const string Tag = "Sripe Payment Methods";
        private const string BasicRoute = "api/";
        private const string ResourceName = "stripe/paymentmethods";
        private ConfigurationStripe keyStripe = new ConfigurationStripe();


        [HttpPost]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(PaymentMethodsResponse))]
        public PaymentMethodsResponse Create(
            [FromUri(Name = "idUserStrpe")] string idUserStrpe,
            [FromUri(Name = "number")] string number,
            [FromUri(Name = "expmonth")] long expmonth,
            [FromUri(Name = "expyear")] long expyear,
            [FromUri(Name = "cvc")] string cvc)
        {
            PaymentMethodsResponse response = new PaymentMethodsResponse();

            try
            {
                try
                {
                    PaymentMethod  method = (new PaymentMethodStripe()).Cretae(keyStripe.skKey, idUserStrpe, number, expmonth, expyear, cvc);
                    response.IdPaymentMethods = method.Id;
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

        [HttpGet]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(PaymentMethodsRegistryResponse))]
        public PaymentMethodsRegistryResponse GetId([FromUri(Name = "idPayMethod")] string idPayMethod)
        {
            PaymentMethodsRegistryResponse response = new PaymentMethodsRegistryResponse();

            try
            {
                try
                {
                    response.paymentmethods = (new PaymentMethodStripe()).ReadId(keyStripe.skKey, idPayMethod);
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


        [HttpGet]
        [Route(BasicRoute + ResourceName + "/list")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(PaymentMethodsListResponse))]
        public PaymentMethodsListResponse GetList([FromUri(Name = "idUserStrpe")] string idUserStrpe)
        {
            PaymentMethodsListResponse response = new PaymentMethodsListResponse();

            try
            {
                try
                {
                    response.paymentmethodList = (new PaymentMethodStripe()).Read(keyStripe.skKey, idUserStrpe);
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


        [HttpPut]
        [Route(BasicRoute + ResourceName + "/attach" )]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(PaymentMethodsAttachDetachResponse))]
        public PaymentMethodsAttachDetachResponse AttachCustomer(
           [FromUri(Name = "idUserStrpe")] string idUserStrpe,
           [FromUri(Name = "idMethod")] string idMethod)
        {
            PaymentMethodsAttachDetachResponse response = new PaymentMethodsAttachDetachResponse();
            try
            {
                try
                {
                    int method = (new PaymentMethodStripe()).AttachPaymentMethodCustomer(keyStripe.skKey, idUserStrpe, idMethod);

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

        [HttpPut]
        [Route(BasicRoute + ResourceName + "/detach")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(PaymentMethodsAttachDetachResponse))]
        public PaymentMethodsAttachDetachResponse DetachCustomer(
         [FromUri(Name = "idMethod")] string idMethod)
        {
            PaymentMethodsAttachDetachResponse response = new PaymentMethodsAttachDetachResponse();
            try
            {
                try
                {
                    int method = (new PaymentMethodStripe()).DetachPaymentMethodCustomer(keyStripe.skKey, idMethod);

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
