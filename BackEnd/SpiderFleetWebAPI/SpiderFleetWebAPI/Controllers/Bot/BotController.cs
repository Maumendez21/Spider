using SpiderFleetWebAPI.Models.Response.Bot;
using SpiderFleetWebAPI.Models.Response.Itineraries;
using SpiderFleetWebAPI.Utils.Bot;
using SpiderFleetWebAPI.Utils.User;
using SpiderFleetWebAPI.Utils.VerifyUser;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Bot
{
    public class BotController : ApiController
    {
        private const string Tag = "Bot";
        private const string BasicRoute = "api/";
        private const string ResourceName = "bot/";

        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName + "consults")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(BotConsultsResponse))]
        public BotConsultsResponse Consults([FromUri(Name = "device")] string device, [FromUri(Name = "type")] int type)
        {
            BotConsultsResponse response = new BotConsultsResponse();

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
                    response = (new BotDao()).Consults(device, hierarchy, type);
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
        [Route(BasicRoute + ResourceName + "speed")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(BotConsultsResponse))]
        public BotConsultsResponse Speed([FromUri(Name = "device")] string device)
        {
            BotConsultsResponse response = new BotConsultsResponse();

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
                    response = (new BotDao()).ConsultsSpeeding(device, hierarchy);
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
        [Route(BasicRoute + ResourceName + "last/position")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(LastPositionResponse))]
        public LastPositionResponse LastPosition([FromUri(Name = "device")] string device)
        {
            LastPositionResponse response = new LastPositionResponse();

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
                    response = (new BotDao()).LastPosition(device);
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
        [Route(BasicRoute + ResourceName + "last/trip")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(StrokeResponse))]
        public async Task<StrokeResponse> LastTrip([FromUri(Name = "device")] string device)
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
                    response = await (new BotDao()).LastTrip(device, hierarchy);
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
        [Route(BasicRoute + ResourceName + "day")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(TotalResponse))]
        public TotalResponse GetKm([FromUri(Name = "device")] string device, [FromUri(Name = "type")] int type)
        {
            TotalResponse response = new TotalResponse();

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
                    response = (new BotDao()).reporteGeneral(hierarchy, device, type);
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
