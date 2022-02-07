using SpiderFleetWebAPI.Models.Response.Itineraries;
using SpiderFleetWebAPI.Utils.Itineraries;
using SpiderFleetWebAPI.Utils.User;
using SpiderFleetWebAPI.Utils.VerifyUser;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Itineraries
{
    public class ItinerariesController : ApiController
    {
        private const string Tag = "Trazo de Viajes";
        private const string BasicRoute = "api/";
        private const string ResourceName = "vehicle/";

        ///Horario de Invierno se aumenta 6 Horas 
        ///Horario de Verano se aumneta 5 Horas
        ///Ejemplo Horario de Invierno
        ///00:00:00 en bdMongo seria 06:00:00 del dia
        ///23:00:00 en bdMongo seria 05:00:00 del siguiente dia 
        ///Ejemplo Horario de Verano
        ///00:00:00 en bdMongo seria 05:00:00 del dia
        ///23:00:00 en bdMongo seria 04:00:00 del siguiente dia 


        /// <summary>
        /// Viajes del Vehiculo
        /// </summary>
        /// <remarks>
        /// Este EndPoint el resumen del viaje, Kilometraje, Gasolina , Tiempo , Calificacion 
        /// #### Ejemplo de entrada
        /// { 
        /// "device" : "213WP2017005868",
        /// "startdate" : "2020-05-04 16:16:34",
        /// "enddate" : "2020-05-04 17:51:56"
        /// }
        /// </remarks>
        /// <example>
        /// ```
        /// {
        /// "device" : "string",
        /// "startdate" : ""DateTime",
        /// "enddate" : "DateTime"
        /// }
        /// ```
        /// </example>
        /// <param name="device"  name="startdate" name="enddate">Objetos de entrada para el Endpoint</param>
        /// <returns>success = true o false;</returns>
        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName + "trips")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ItinerariesResponse))]
        public ItinerariesResponse ReadItinerariesDevice([FromUri(Name = "device")] string device, 
            [FromUri(Name = "startdate")] DateTime startdate, [FromUri(Name = "enddate")] DateTime enddate)
        {
            ItinerariesResponse response = new ItinerariesResponse();

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
                    response = (new ItinerariesDao()).ReadItinerariosDeviceList(hierarchy, device, startdate, enddate);
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
        /// Ruta y Alarmas del Vehiculo durante el viaje
        /// </summary>
        /// <remarks>
        /// Este EndPoint el resumen del viaje, Kilometraje, Gasolina , Tiempo , Calificacion 
        /// #### Ejemplo de entrada
        /// { 
        /// "device" : "213WP2017005868",
        /// "startdate" : "2020-05-04 16:16:34",
        /// "enddate" : "2020-05-04 17:51:56"
        /// }
        /// </remarks>
        /// <example>
        /// ```
        /// {
        /// "device" : "string",
        /// "startdate" : ""DateTime",
        /// "enddate" : "DateTime"
        /// }
        /// ```
        /// </example>
        /// <param name="device"  name="startdate" name="enddate">Objetos de entrada para el Endpoint</param>
        /// <returns>success = true o false;</returns>
        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName + "stroke")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(StrokeResponse))]
        public async Task<StrokeResponse> ReadStrokeDevice([FromUri(Name = "device")] string device,
           [FromUri(Name = "startdate")] string startdate, [FromUri(Name = "enddate")] string enddate
            )
        {
            StrokeResponse response = new StrokeResponse();

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
                    response = await (new ItinerariesDao()).ReadStrokeDeviceList(device, startdate, enddate, hierarchy);
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
        /// Los Ultimos 5 Viajes del Vehiculo
        /// </summary>
        /// <remarks>
        /// Este EndPoint el resumen del viaje, Kilometraje, Gasolina , Tiempo , Calificacion 
        /// #### Ejemplo de entrada
        /// { 
        /// "device" : "213WP2017005868"
        /// }
        /// </remarks>
        /// <example>
        /// ```
        /// {
        /// "device" : "string"
        /// }
        /// ```
        /// </example>
        /// <param name="device">Objetos de entrada para el Endpoint</param>
        /// <returns>success = true o false;</returns>
        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName + "list/trips")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ItinerariesListLastResponse))]
        public ItinerariesListLastResponse ReadTripsFiveDevice([FromUri(Name = "device")] string device)
        {
            ItinerariesListLastResponse response = new ItinerariesListLastResponse();

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
                    response = (new ItinerariesDao()).ReadTripsRecursive(response, hierarchy, device);//, startdate, enddate);
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
        /// Obtiene los datos generales del dia
        /// </summary>
        /// <remarks>
        /// Este EndPoint el resumen del viaje, Kilometraje, Gasolina , Tiempo , Calificacion 
        /// #### Ejemplo de entrada
        /// { 
        /// "device" : "213WP2017005868"
        /// }
        /// </remarks>
        /// <example>
        /// ```
        /// {
        /// "device" : "string"
        /// }
        /// ```
        /// </example>
        /// <param name="device">Objetos de entrada para el Endpoint</param>
        /// <returns>success = true o false;</returns>
        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName + "general/information/trip")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ItinerariesGeneralResponse))]
        public ItinerariesGeneralResponse ReadItinerariesDevice([FromUri(Name = "device")] string device)
        {
            ItinerariesGeneralResponse response = new ItinerariesGeneralResponse();

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
                    response = (new ItinerariesDao()).ReadGeneralDeviceList(hierarchy, device);
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

        ///
        /// <summary>
        /// Ruta y Alarmas del Vehiculo durante el viaje link para el reporte sin seguridad
        /// </summary>
        /// <remarks>
        /// Este EndPoint el resumen del viaje, Kilometraje, Gasolina , Tiempo , Calificacion 
        /// #### Ejemplo de entrada
        /// { 
        /// "device" : "213WP2017005868",
        /// "startdate" : "2020-05-04 16:16:34",
        /// "enddate" : "2020-05-04 17:51:56"
        /// }
        /// </remarks>
        /// <example>
        /// ```
        /// {
        /// "device" : "string",
        /// "startdate" : ""DateTime",
        /// "enddate" : "DateTime"
        /// }
        /// ```
        /// </example>
        /// <param name="device"  name="startdate" name="enddate">Objetos de entrada para el Endpoint</param>
        /// <returns>success = true o false;</returns>
        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName + "link")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(StrokeResponse))]
        public async Task<StrokeResponse> ReadStrokeLink([FromUri(Name = "device")] string device,
           [FromUri(Name = "startdate")] string startdate, [FromUri(Name = "enddate")] string enddate)
        {
            StrokeResponse response = new StrokeResponse();

            try
            {
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

                    response = await (new ItinerariesDao()).ReadStrokeDeviceList(device, startdate, enddate, hierarchy);
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


