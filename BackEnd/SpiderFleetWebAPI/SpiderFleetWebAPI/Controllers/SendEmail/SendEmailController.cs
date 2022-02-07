using SpiderFleetWebAPI.Models.Response.Email;
using SpiderFleetWebAPI.Utils.Email;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.SendEmail
{
    public class SendEmailController : ApiController
    {

        private const string Tag = "Rutas";
        private const string BasicRoute = "api/";
        private const string ResourceName = "send/email";

        [HttpGet]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(EmialResponse))]
        public EmialResponse GetIdGeoFence([FromUri(Name = "email")] string email)
        {
            EmialResponse response = new EmialResponse();

            try
            {
                response = (new EmailDao()).SendEmial(email);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }
            return response;
        }

    }
}
