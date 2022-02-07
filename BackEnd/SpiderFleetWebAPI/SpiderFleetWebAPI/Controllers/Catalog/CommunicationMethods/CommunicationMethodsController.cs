using SpiderFleetWebAPI.Models.Response.Catalog.CommunicationMethods;
using SpiderFleetWebAPI.Utils.Catalog.CommunicationMethods;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Catalog.CommunicationMethods
{
    public class CommunicationMethodsController : ApiController
    {

        private const string Tag = "Mantenimiento de Metodos de Comunicacion";
        private const string BasicRoute = "api/";
        private const string ResourceName = "configuration/communication/methods";

        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName + "/list")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(CommunicationMethodsListResponse))]
        public CommunicationMethodsListResponse GetListAsync()
        {
            CommunicationMethodsListResponse response = new CommunicationMethodsListResponse();
            try
            {
                try
                {
                    response = (new CommunicationMethodsDao()).Read();
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
