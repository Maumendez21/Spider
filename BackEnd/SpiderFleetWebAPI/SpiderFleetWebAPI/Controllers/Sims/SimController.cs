using SpiderFleetWebAPI.Models.Response.Sims;
using SpiderFleetWebAPI.Utils.Sims;
using SpiderFleetWebAPI.Utils.VerifyUser;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Net;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Sims
{
    public class SimController : ApiController
    {
        private const string Tag = "Sim disponibles";
        private const string BasicRoute = "api/";
        private const string ResourceName = "sim";

        /// <summary>
        /// Sims diponibles 
        /// </summary>
        /// <remarks>
        /// Este EndPoint el devuelve los sims que estan disponibles
        /// #### Ejemplo de entrada
        /// </remarks>
        /// <example>
        /// ```
        /// </example>
        /// <param>Objetos de entrada para el Endpoint</param>
        /// <returns>success = true o false;</returns>
        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName + "/available")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(SimsResponse))]
        public SimsResponse Available()
        {
            SimsResponse response = new SimsResponse();

            try
            {
                string username = string.Empty;
                username = (new VerifyUser()).verifyTokenUser(User);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }

            try
            {
                try
                {
                    response = (new SimsDao()).ReadAvailable();
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
