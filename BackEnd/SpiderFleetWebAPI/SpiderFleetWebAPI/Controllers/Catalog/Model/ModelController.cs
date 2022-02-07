using SpiderFleetWebAPI.Models.Response.Catalog.Model;
using SpiderFleetWebAPI.Utils.Catalog.Model;
using SpiderFleetWebAPI.Utils.VerifyUser;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Net;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Catalog.Model
{
    public class ModelController : ApiController
    {
        private const string Tag = "Mantenimiento de Modelos de Vehiculos";
        private const string BasicRoute = "api/";
        private const string ResourceName = "administration/models";

        /// <summary>
        /// Lista de Modelos de Autos
        /// </summary>
        /// <remarks>
        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName + "/list")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ModelListResponse))]
        public ModelListResponse GetListAsync()
        {
            ModelListResponse response = new ModelListResponse();
            try
            {
                string username = string.Empty;
                //string hierarchy = string.Empty;

                try
                {
                    username = (new VerifyUser()).verifyTokenUser(User);
                    //hierarchy = (new UserDao()).ReadUserHierarchy(username);
                }
                catch (Exception ex)
                {
                    response.success = false;
                    response.messages.Add(ex.Message);
                    return response;
                }

                try
                {
                    response = (new ModelDao()).Read();
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
