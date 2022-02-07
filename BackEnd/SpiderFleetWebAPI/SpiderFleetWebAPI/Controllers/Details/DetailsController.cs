using SpiderFleetWebAPI.Models.Request.Details;
using SpiderFleetWebAPI.Models.Response.Details;
using SpiderFleetWebAPI.Utils.Details;
using SpiderFleetWebAPI.Utils.User;
using SpiderFleetWebAPI.Utils.VerifyUser;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Details
{
    public class DetailsController : ApiController
    {
        private const string Tag = "Mantenimiento de Detalles de Dispositivo";
        private const string BasicRoute = "api/";
        private const string ResourceName = "configuration/details";

        [Authorize]
        [HttpPost]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(DetailsResponse))]
        public DetailsResponse Create([FromBody] DetailsRequest request)
        {
            DetailsResponse response = new DetailsResponse();

            try
            {
                if (!(request is DetailsRequest))
                {
                    response.success = false;
                    response.messages.Add("Objeto de entrada invalido");
                    return response;
                }

                string hierarchy = string.Empty;
                string username = string.Empty;

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

                CredencialSpiderFleet.Models.Details.Details details = new CredencialSpiderFleet.Models.Details.Details();
                details.Device = request.Device;
                details.TypeDevice = request.TypeDevice;
                details.Model = request.Model;
                details.IdCommunicationMethod = request.IdCommunicationMethod;
                details.Batery = request.Batery;
                details.BatteryDuration = request.BatteryDuration;
                details.IdSamplingTime = request.IdSamplingTime;
                details.Motorized = request.Motorized;
                details.Performance = request.Performance.ToString();

                try
                {
                    response = (new DetailsDao()).Create(details);

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
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(DetailsRegistryResponse))]
        public DetailsRegistryResponse GetListIdAsync([FromUri(Name = "device")] string device)
        {
            DetailsRegistryResponse response = new DetailsRegistryResponse();
            try
            {
                string hierarchy = string.Empty;
                string username = string.Empty;

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
                    response = (new DetailsDao()).ReadId(device);
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
        [Route(BasicRoute + ResourceName + "/list")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(DetailsListResponse))]
        public DetailsListResponse GetListAsync([FromUri(Name = "search")] string search)
        {
            DetailsListResponse response = new DetailsListResponse();
            try
            {
                string hierarchy = string.Empty;
                string username = string.Empty;

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
                    response = (new DetailsDao()).Read(hierarchy, search);
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
