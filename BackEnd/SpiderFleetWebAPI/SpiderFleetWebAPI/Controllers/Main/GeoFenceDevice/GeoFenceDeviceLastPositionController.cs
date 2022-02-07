using SpiderFleetWebAPI.Models.Response.Main.GeoFenceDevice;
using SpiderFleetWebAPI.Utils.Main.GeoFenceDevice;
using SpiderFleetWebAPI.Utils.User;
using SpiderFleetWebAPI.Utils.VerifyUser;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Net;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Main.GeoFenceDevice
{
    public class GeoFenceDeviceLastPositionController : ApiController
    {

        private const string Tag = "Consulta de Geo Cercas y Ultima posicion del Vehiculo";
        private const string BasicRoute = "api/";
        private const string ResourceName = "geo/fence/monitoring";

        /// <summary>
        /// Obtiene la lista de GeoCercas que pertenecen a una Empresa
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName + "/list")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(GeoFenceListResponse))]
        public GeoFenceListResponse GetListGeoFences()
        {
            GeoFenceListResponse response = new GeoFenceListResponse();

            string hierarchy = string.Empty;
            try
            {
                string username = string.Empty;
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
                response = (new GeoFenceDeviceLastPositionDao()).ReadGeoFences(hierarchy);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }

            return response;
        }

        /// <summary>
        /// Obtiene todos los dispositivos que pertenecen a una Geo Cerca
        /// </summary>
        /// <param name="id">Es el Id de la Geo Cerca</param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName + "/last/positions")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(LastStatusDeviceGeoFenceResponse))]
        public LastStatusDeviceGeoFenceResponse GetListLastStatusDevices([FromUri(Name = "id")] string id)
        {
            LastStatusDeviceGeoFenceResponse response = new LastStatusDeviceGeoFenceResponse();

            string hierarchy = string.Empty;
            try
            {
                string username = string.Empty;
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
                response = (new GeoFenceDeviceLastPositionDao()).ReadLastPositionDevice(id);
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
