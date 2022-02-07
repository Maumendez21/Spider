using SpiderFleetWebAPI.Models.Response.Stripe;
using Stripe;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Stripe
{
    public class StripeController : ApiController
    {
        private const string Tag = "Sripe Pagos";
        private const string BasicRoute = "api/";
        private const string ResourceName = "stripe/checkot";

        private const string endpointSecret = "sk_test_BwrJByBAx9AMIpN9Q58rpyNB00m5inVKlZ";


        //[HttpPost]
        //[Route(BasicRoute + ResourceName)]
        //[SwaggerOperation(Tags = new[] { Tag })]
        //[SwaggerResponse(HttpStatusCode.OK, Type = typeof(StripeCustomerResponse))]
        //public StripeCustomerResponse Create([FromUri(Name = "stripeToken")] string stripeToken)
        //{
        //    StripeCustomerResponse response = new StripeCustomerResponse();
        //    try
        //    {
        //        StripeConfiguration.ApiKey = "sk_test_BwrJByBAx9AMIpN9Q58rpyNB00m5inVKlZ";
        //        var options = new ChargeCreateOptions
        //        {
        //            Amount = 2000,
        //            Currency = "mxn",
        //            Description = "Charge for jenny.rose@example.com",
        //            Source = stripeToken
        //        };

        //        var service = new ChargeService();
        //        Charge charge = service.Create(options);
        //        response.success = true;
        //        return response; // RedirectToAction("Index");
        //    }
        //    catch(Exception ex)
        //    {
        //        //return View();
        //        response.success = false;
        //        response.messages.Add(ex.Message);
        //        return response;
        //    }
        //}


        //[HttpPost]
        //[Route(BasicRoute + ResourceName)]
        //[SwaggerOperation(Tags = new[] { Tag })]
        //[SwaggerResponse(HttpStatusCode.OK, Type = typeof(StripeProyectResponse))]
        //public async Task<StripeProyectResponse> Index([FromBody]string stripeToken)
        //{
        //    StripeConfiguration.ApiKey = "pk_test_3EXIMqBGoeVV0bgKrBObly0q00VnKTr2NF";
        //    StripeProyectResponse response = new StripeProyectResponse();


        //    var options = new ChargeCreateOptions
        //    {
        //        Amount = 2000,
        //        Currency = "mxn",
        //        Description = "Cargo for  jenny.rose@example.com",
        //        Source = stripeToken
        //        //PaymentMethodTypes = new List<string>
        //        //{
        //        //    "card",
        //        //},
        //    };
        //    var service = new ChargeService();
        //    Charge charge = service.Create(options);

        //    return response;

        //    //var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        //    //var json = await new StreamReader(HttpContext.Current.Request.InputStream).ReadToEndAsync();

        //    //try
        //    //{
        //    //    var stripeEvent = EventUtility.ParseEvent(json);

        //    //    // Handle the event
        //    //    if (stripeEvent.Type == Events.PaymentIntentSucceeded)
        //    //    {
        //    //        var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
        //    //        Console.WriteLine("PaymentIntent was successful!");
        //    //    }
        //    //    else if (stripeEvent.Type == Events.PaymentMethodAttached)
        //    //    {
        //    //        var paymentMethod = stripeEvent.Data.Object as PaymentMethod;
        //    //        Console.WriteLine("PaymentMethod was attached to a Customer!");
        //    //    }
        //    //    // ... handle other event types
        //    //    else
        //    //    {
        //    //        // Unexpected event type
        //    //        //return BadRequest();
        //    //        response.success = false;
        //    //        response.messages.Add("");
        //    //        return response;
        //    //    }

        //    //    //return Ok();
        //    //    return response;
        //    //}
        //    //catch (StripeException e)
        //    //{
        //    //    //return BadRequest();
        //    //    response.success = false;
        //    //    response.messages.Add(e.Message);
        //    //    return response;
        //    //}
        //}

        //public void ddd()
        //{

        //    StripeConfiguration.ApiKey = "sk_test_BwrJByBAx9AMIpN9Q58rpyNB00m5inVKlZ";

        //    var options = new PaymentIntentCreateOptions
        //    {
        //        Amount = 2000,
        //        Currency = "mxn",
        //        PaymentMethodTypes = new List<string>
        //        {
        //            "card",
        //        },
        //    };
        //    var service = new PaymentIntentService();
        //    service.Create(options);

        //}

        //[HttpPut]
        //[Route(BasicRoute + ResourceName)]
        //[SwaggerOperation(Tags = new[] { Tag })]
        //[SwaggerResponse(HttpStatusCode.OK, Type = typeof(StripeProyectResponse))]
        //public async Task<StripeProyectResponse> AcceptPayment([FromBody] ConfirmPaymentRequest confirmRequest)
        //{
        //    StripeProyectResponse response = new StripeProyectResponse();

        //    PaymentIntent intent = null;
        //    PayResponseBody responseBody = new PayResponseBody();
        //    //PaymentIntentCreateParams.Builder createParamsBuilder = new PaymentIntentCreateParams.Builder();
        //    //PaymentIntentCreateParams createParams = new PaymentIntentCreateParams();

        //    try
        //    {
        //        if (string.IsNullOrEmpty(confirmRequest.getPaymentMethodId))
        //        {

        //            if (!confirmRequest.getUseStripeSdk)
        //            {
        //                response.success = false;
        //                response.messages.Add("");
        //                return response;
        //            }


        //        }

        //        if (string.IsNullOrEmpty(confirmRequest.getPaymentIntentId))
        //        {
        //            intent = PaymentIntent.FromJson(confirmRequest.getPaymentIntentId);
        //            //intent = intent.c  confirm();
        //        }



        //    }
        //    catch (Exception e)
        //    {
        //        response.success = false;
        //        response.messages.Add(e.Message);
        //        return response;
        //    }
        //    try
        //    {




        //        responseBody = (new StripeGenerateResponse()).generateResponse(intent, responseBody);
        //    }
        //    catch(Exception ex)
        //    {

        //    }

        //    StripeConfiguration.ApiKey = "pk_test_3EXIMqBGoeVV0bgKrBObly0q00VnKTr2NF";
        //    //var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        //    var json = await new StreamReader(HttpContext.Current.Request.InputStream).ReadToEndAsync();

        //    try
        //    {
        //        var stripeEvent = EventUtility.ParseEvent(json);

        //        // Handle the event
        //        if (stripeEvent.Type == Events.PaymentIntentSucceeded)
        //        {
        //            var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
        //            Console.WriteLine("PaymentIntent was successful!");
        //        }
        //        else if (stripeEvent.Type == Events.PaymentMethodAttached)
        //        {
        //            var paymentMethod = stripeEvent.Data.Object as PaymentMethod;
        //            Console.WriteLine("PaymentMethod was attached to a Customer!");
        //        }
        //        // ... handle other event types
        //        else
        //        {
        //            response.success = false;
        //            response.messages.Add("");
        //            return response;
        //        }

        //        response.success = true;                    

        //        return response;
        //    }
        //    catch (StripeException e)
        //    {
        //        response.success = false;
        //        response.messages.Add(e.Message);
        //        return response;
        //    }
        //}
    }
}