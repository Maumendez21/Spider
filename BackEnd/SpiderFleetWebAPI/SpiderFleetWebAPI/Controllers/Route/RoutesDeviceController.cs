using SpiderFleetWebAPI.Models.Request.Routes;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Route
{
    public class RoutesDeviceController : ApiController
    {
        /*
        private const string Tag = "Asignacion de Geo Cercas";
        private const string BasicRoute = "api/";
        private const string ResourceName = "asignacion/routes";

        [Authorize]
        [HttpPost]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(GeoFenceDeviceResponse))]
        public GeoFenceDeviceResponse CreateGeoFence(
           [FromBody] RouteDeviceRequest geoFence)
        {
            GeoFenceDeviceResponse response = new GeoFenceDeviceResponse();

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

            if (!(geoFence is GeoFenceDeviceRequest))
            {
                response.success = false;
                response.messages.Add("Objeto de entrada invalido");
                return response;
            }

            CredencialSpiderFleet.Models.Main.GeoFenceDevice.GeoFenceDevice geoFences = new CredencialSpiderFleet.Models.Main.GeoFenceDevice.GeoFenceDevice();
            geoFences.IdGeoFence = geoFence.IdGeoFence;
            geoFences.ListDevice = geoFence.ListDevice;

            try
            {
                response = (new GeoFenceDeviceDao()).Create(geoFences);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }

            return response;
        }

        [Authorize]
        [HttpPost]
        [Route(BasicRoute + ResourceName + "/device")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(GeoFenceDeviceListIdResponse))]
        public GeoFenceDeviceListIdResponse DeleteGeoFence(
           [FromBody] RouteDeviceListIdRequest geoFence)
        {
            GeoFenceDeviceListIdResponse response = new GeoFenceDeviceListIdResponse();

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

            if (!(geoFence is GeoFenceDeviceListIdRequest))
            {
                response.success = false;
                response.messages.Add("Objeto de entrada invalido");
                return response;
            }

            CredencialSpiderFleet.Models.Main.GeoFenceDevice.GeoFenceDeviceId geoFences = new CredencialSpiderFleet.Models.Main.GeoFenceDevice.GeoFenceDeviceId();
            geoFences.ListDeviceId = geoFence.ListDeviceId;

            try
            {
                response = (new GeoFenceDeviceDao()).Delete(geoFences);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }

            return response;
        }

        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(GeoFenceDeviceListResponse))]
        public GeoFenceDeviceListResponse GetListGeoFence()
        {
            GeoFenceDeviceListResponse response = new GeoFenceDeviceListResponse();

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
                response = (new GeoFenceDeviceDao()).Read(hierarchy);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }

            return response;
        }
        */
    }

}
