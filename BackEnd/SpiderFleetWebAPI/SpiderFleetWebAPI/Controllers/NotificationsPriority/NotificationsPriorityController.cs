using SpiderFleetWebAPI.Models.Response.NotificationsPriority;
using SpiderFleetWebAPI.Utils.NotificationsPriority;
using SpiderFleetWebAPI.Utils.VerifyUser;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.NotificationsPriority
{
    public class NotificationsPriorityController : ApiController
    {
        private const string Tag = "Catalogo de Notificaciones";
        private const string BasicRoute = "api/";
        private const string ResourceName = "notifications/priority";


        /// <summary>
        /// Actualiza la bandera de la notificacion que se ha mostrado
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <example>
        /// </example>
        /// <param name="id"
        [Authorize]
        [HttpPut]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(NotificationsPriorityResponse))]
        public NotificationsPriorityResponse ReadItinerariesDevice([FromUri(Name = "id")] int id)
        {
            NotificationsPriorityResponse response = new NotificationsPriorityResponse();

            try
            {
                string username = string.Empty;
                string hierarchy = string.Empty;

                try
                {
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
                    response = (new NotificationsPriorityDao()).Update(id);
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
