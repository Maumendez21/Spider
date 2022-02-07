using SpiderFleetWebAPI.Models.Request.PointsInterest;
using SpiderFleetWebAPI.Models.Response.PointsInterest;
using SpiderFleetWebAPI.Utils.PointInterest;
using SpiderFleetWebAPI.Utils.User;
using SpiderFleetWebAPI.Utils.VerifyUser;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.PointInterest
{
    public class PointInterestDeviceController : ApiController
    {

        private const string Tag = "Asignacion de Puntos de Interes";
        private const string BasicRoute = "api/";
        private const string ResourceName = "assignment/point/interest";

        [Authorize]
        [HttpPost]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(PointInterestDeviceResponse))]
        public PointInterestDeviceResponse Post(
           [FromBody] PointInterestDeviceRequest request)
        {
            PointInterestDeviceResponse response = new PointInterestDeviceResponse();

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

            if (!(request is PointInterestDeviceRequest))
            {
                response.success = false;
                response.messages.Add("Objeto de entrada invalido");
                return response;
            }

            CredencialSpiderFleet.Models.PointInterest.PointInterestDevice geoFences = new CredencialSpiderFleet.Models.PointInterest.PointInterestDevice();
            geoFences.IdPointInterest = request.IdPointInterest;
            geoFences.ListDevice = request.ListDevice;

            try
            {
                response = (new PointInterestDeviceDao()).Create(geoFences);
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
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(PointInsterestDeviceListIdResponse))]
        public PointInsterestDeviceListIdResponse DeleteGeoFence(
           [FromBody] PointInterestDeviceListIdRequest request)
        {
            PointInsterestDeviceListIdResponse response = new PointInsterestDeviceListIdResponse();

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

            if (!(request is PointInterestDeviceListIdRequest))
            {
                response.success = false;
                response.messages.Add("Objeto de entrada invalido");
                return response;
            }

            CredencialSpiderFleet.Models.PointInterest.PointInterestDeviceId devices = new CredencialSpiderFleet.Models.PointInterest.PointInterestDeviceId();
            devices.ListDeviceId = request.ListDeviceId;

            try
            {
                response = (new PointInterestDeviceDao()).Delete(devices);
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
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(PointInterestDeviceListResponse))]
        public PointInterestDeviceListResponse GetListGeoFence()
        {
            PointInterestDeviceListResponse response = new PointInterestDeviceListResponse();

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
                response = (new PointInterestDeviceDao()).Read(hierarchy);
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
