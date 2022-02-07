using SpiderFleetWebAPI.Models.Response.Main.LastPositionDevice;
using SpiderFleetWebAPI.Utils.Main.LastPositionDevice;
using SpiderFleetWebAPI.Utils.User;
using SpiderFleetWebAPI.Utils.VerifyUser;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Main.LastPositionDevice
{
    public class LastPositionDeviceController : ApiController
    {

        private const string Tag = "Listado de Ultima posicion de Dispositivos";
        private const string BasicRoute = "api/";
        private const string ResourceName = "main/last/position";

        /// <summary>
        /// Lista de dispositivos
        /// </summary>
        /// <remarks>
        /// Este EndPoint regresa una lista de dispositivos 
        /// #### Ejemplo de entrada
        /// { 
        /// "tipo" : "Flota",  --> Vehiculo
        /// "valor" : "38"     --> 213WP2017005868
        /// }
        /// </remarks>
        /// <example>
        /// ```
        /// {
        /// "tipo" : "string",
        /// "valor" : ""string"
        /// }
        /// ```
        /// </example>
        /// <param name="tipo" name="valor">Objeto de entrada para el Endpoint</param>
        /// <returns>success = true o false;</returns>
        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName + "/list/device")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(LastPositionDeviceResponse))]
        public LastPositionDeviceResponse GetListLastPositionDevice([FromUri(Name = "tipo")]string tipo, [FromUri(Name = "valor")]string valor, [FromUri(Name = "busqueda")] string busqueda)
        {
            LastPositionDeviceResponse response = new LastPositionDeviceResponse();

            try
            {
                string username = string.Empty;

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
                    if(valor.Equals("83"))
                    {
                        response = (new LastPositionDevicesDao()).ReadDemo(tipo, busqueda);
                    }
                    else
                    {
                        response = (new LastPositionDevicesDao()).ReadCurrentPositionDevices(tipo, valor, busqueda);
                    }                    
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


        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName + "/devices")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(LastPositionDeviceResponse))]
        public async Task<LastPositionDeviceResponse> GetListLastPositionDevice([FromUri(Name = "busqueda")] string busqueda)
        {
            LastPositionDeviceResponse response = new LastPositionDeviceResponse();

            try
            {
                string username = string.Empty;
                string hierarchy = string.Empty;

                try
                {
                    username = (new VerifyUser()).verifyTokenUser(User);
                    hierarchy = (new UserDao()).ReadUserHierarchy(username);
                }
                catch (Exception ex)
                {
                    response.success = false;
                    response.messages.Add(ex.Message);
                    return response;
                }

                try
                {
                    if (hierarchy.Equals("/83/"))
                    {
                        response = (new LastPositionDevicesDao()).ReadDemo("Float", busqueda);
                    }
                    else
                    {
                        response = await (new LastPositionDevicesDao()).ReadCurrentPositionDevicesHierarchy(hierarchy, busqueda);
                    }
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


        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName + "/notifications/priority")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(NotificationsResponse))]
        public NotificationsResponse GetListNotificationsPriority()
        {
            NotificationsResponse response = new NotificationsResponse();

            try
            {
                string username = string.Empty;
                string hierarchy = string.Empty;

                try
                {
                    username = (new VerifyUser()).verifyTokenUser(User);
                    hierarchy = (new UserDao()).ReadUserHierarchy(username);
                }
                catch (Exception ex)
                {
                    response.success = false;
                    response.messages.Add(ex.Message);
                    return response;
                }

                try
                {
                    response = (new LastPositionDevicesDao()).NotificationsPriority(hierarchy);
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
