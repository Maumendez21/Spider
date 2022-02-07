using SpiderFleetWebAPI.Models.Response.Stripe;
using Stripe;
using Stripe.Checkout;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Stripe
{
    public class CustomersController : ApiController
    {

        //private const string Tag = "Sripe Customers";
        //private const string BasicRoute = "api/";
        //private const string ResourceName = "stripe/customers";

        //private const string endpointSecret = "sk_test_BwrJByBAx9AMIpN9Q58rpyNB00m5inVKlZ";

        //[HttpPost]
        //[Route(BasicRoute + ResourceName)]
        //[SwaggerOperation(Tags = new[] { Tag })]
        //[SwaggerResponse(HttpStatusCode.OK, Type = typeof(StripeProyectResponse))]
        //public StripeProyectResponse PaymentMethod()
        //{
        //    StripeProyectResponse response = new StripeProyectResponse();
        //    try
        //    {

        //        StripeConfiguration.ApiKey = endpointSecret;

        //        var options = new PaymentMethodCreateOptions
        //        {
        //            Type = "card",
        //            Card = new PaymentMethodCardCreateOptions //  PaymentMethodCardOptions
        //            {
        //                Number = "4242424242424242",
        //                ExpMonth = 1,
        //                ExpYear = 2021,
        //                Cvc = "314",
        //            },
        //        };
        //        var service = new PaymentMethodService();
        //        service.Create(options);


        //        response.success = true;
        //        return response; // RedirectToAction("Index");
        //    }
        //    catch (Exception ex)
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
        //public StripeProyectResponse Invoice()
        //{
        //    StripeProyectResponse response = new StripeProyectResponse();
        //    try
        //    {

        //        StripeConfiguration.ApiKey = endpointSecret;

        //        var options = new InvoiceItemCreateOptions
        //        {
        //            Customer = "cus_GaaZgzLlohLrGw",
        //            Amount = 10000,
        //            Currency = "mxn",
        //            Description = "One-time setup fee",
        //        };
        //        var service = new InvoiceItemService();
        //        service.Create(options);
        //        var invoiceOptions = new InvoiceCreateOptions
        //        {
        //            Customer = "cus_GaaZgzLlohLrGw",
        //            AutoAdvance = true,
        //        };
        //        var invoiceService = new InvoiceService();
        //        invoiceService.Create(invoiceOptions);


        //        response.success = true;
        //        return response; // RedirectToAction("Index");
        //    }
        //    catch (Exception ex)
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
        //public StripeProyectResponse Session()
        //{
        //    StripeProyectResponse response = new StripeProyectResponse();
        //    try
        //    {

        //        StripeConfiguration.ApiKey = endpointSecret;

        //        var options = new SessionCreateOptions
        //        {
        //            CustomerEmail = "jenny.rivera@example.com",
        //            PaymentMethodTypes = new List<string> {
        //            "card",
        //        },
        //                        LineItems = new List<SessionLineItemOptions> {
        //            new SessionLineItemOptions {
        //                Name = "Condutas de Majeo",
        //                Description = "Servicio de Conductas de manejo y monitoreo de Vehiculos",
        //                Amount = 10000,
        //                Currency = "mxn",
        //                Quantity = 1,
        //            },
        //        },
        //            SuccessUrl = "https://example.com/success",
        //            CancelUrl = "https://example.com/cancel",
        //        };

        //        var service = new SessionService();
        //        Session session = service.Create(options);


        //        response.success = true;
        //        return response; // RedirectToAction("Index");
        //    }
        //    catch (Exception ex)
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
        //public StripeProyectResponse Customer([FromUri(Name = "stripeToken")] string stripeToken)
        //{
        //    StripeProyectResponse response = new StripeProyectResponse();
        //    try
        //    {
        //        StripeConfiguration.ApiKey = "sk_test_BwrJByBAx9AMIpN9Q58rpyNB00m5inVKlZ";


        //        //Creacion de Cliente
        //        var options = new CustomerCreateOptions
        //        {
        //            Email = "angela@hotmail.com",
        //            Description = "Fatura Datos Completos alta de usuario",
        //            Name = "Angela Aguilar",
        //            Address = new AddressOptions
        //            {
        //                Line1 = "",
        //                City = "Puebla",
        //                Country = "Mexico",
        //                Line2 = "",
        //                PostalCode = "72000",
        //                State = "Puebla"
        //            },
        //            Phone = "2233993377",
        //            //Shipping
                    
        //        };
        //        var service = new CustomerService();
        //        Customer customer = service.Create(options);

        //        string idCustomer = customer.Id;

        //        //
        //        var optionsInvoice = new InvoiceItemCreateOptions
        //        {
        //            Customer = idCustomer,
        //            Amount = 900000,
        //            Currency = "mxn",
        //            Description = "Facturacion Impuestos de Monitoreo",
        //            TaxRates = new List<string> {
        //                "txr_1G3VLHGI3wXPbnaMFUp2WZ0p"
        //             },
                    
                 
        //            //Invoice = "2000",

        //            //TaxRates = new TaxRateCreateOptions
        //            //{ 
        //            //    Percentage = "",
        //            //}, 
        //        };
        //        var serviceInvoice = new InvoiceItemService();
        //        serviceInvoice.Create(optionsInvoice);
        //        var invoiceOptions = new InvoiceCreateOptions
        //        {
        //            Customer = idCustomer, 
        //            AutoAdvance = false,    //you can continue to modify the invoice until you finalize it
        //        };
        //        var invoiceService = new InvoiceService();
        //        invoiceService.Create(invoiceOptions);

        //        //var optionsSend = new InvoiceItemCreateOptions
        //        //{
        //        //    Amount = 1000,
        //        //    Currency = "mxn",
        //        //    Customer = "cus_4fdAW5ftNQow1a",
        //        //    Description = "One-time setup fee",
        //        //};
        //        //var serviceSend = new InvoiceItemService();
        //        //InvoiceLineItem invoiceItem = serviceSend.Create(optionsSend);


        //        //var serviceFin = new InvoiceService();
        //        //serviceFin.FinalizeInvoice("in_18jwqyLlRB0eXbMtrUQ97YBw");


        //        response.success = true;
        //        return response; // RedirectToAction("Index");
        //    }
        //    catch (Exception ex)
        //    {
        //        //return View();
        //        response.success = false;
        //        response.messages.Add(ex.Message);
        //        return response;
        //    }
        //}

    }
}
