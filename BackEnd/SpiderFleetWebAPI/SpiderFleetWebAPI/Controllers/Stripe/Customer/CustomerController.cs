using SpiderFleetStripe.Configuration;
using SpiderFleetStripe.Stripe.Customers;
using SpiderFleetWebAPI.Models.Response.Stripe.Customers;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Net;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Stripe.Customer
{
    public class CustomerController : ApiController
    {

        private const string Tag = "Sripe Customers";
        private const string BasicRoute = "api/";
        private const string ResourceName = "stripe/customers";
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
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(StripeCustomerRegistryResponse))]
        public StripeCustomerRegistryResponse Create(
            [FromUri(Name = "email")] string email,
            [FromUri(Name = "description")] string description,
            [FromUri(Name = "name")] string name,
            [FromUri(Name = "phone")] string phone)
        {
            StripeCustomerRegistryResponse response = new StripeCustomerRegistryResponse();

            try
            {
                try
                {
                    response.customer = (new CustomerStripe()).Cretae(keyStripe.skKey, email, description, name, phone);
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

        /// <summary>
        /// Obtiene un registro por Id de la tabla Compañia
        /// </summary>
        /// <remarks>
        /// Este EndPoint obtiene un registro
        /// <param name="id">Id del registro a consultar</param>
        /// <returns>Es un objeto de Compañia estructura similar a la tabla Compañia</returns>
        [HttpGet]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(StripeCustomerRegistryResponse))]
        public StripeCustomerRegistryResponse GetId([FromUri(Name = "id")] string id)
        {
            StripeCustomerRegistryResponse response = new StripeCustomerRegistryResponse();
            
            try
            {
                try
                {
                    response.customer = (new CustomerStripe()).ReadId(keyStripe.skKey, id);
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

        /// <summary>
        /// Metodo que regresa la lista de usuarios
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(BasicRoute + ResourceName + "/list")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(StripeCustomerListResponse))]
        public StripeCustomerListResponse GetList()
        {
            StripeCustomerListResponse response = new StripeCustomerListResponse();

            try
            {
                try
                {
                    response.customer = (new CustomerStripe()).Read(keyStripe.skKey);
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
