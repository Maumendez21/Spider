using SpiderFleetWebAPI.Models.Response.EngineStop;
using SpiderFleetWebAPI.Utils.EngineStop;
using SpiderFleetWebAPI.Utils.User;
using SpiderFleetWebAPI.Utils.VerifyUser;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.EngineStop
{
    public class EngineStopController : ApiController
    {
        private const string Tag = "Paro de Motor";
        private const string BasicRoute = "api/";
        private const string ResourceName = "engine/stop";

        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(EngineStopRegistryResponse))]
        public EngineStopRegistryResponse ListEvents([FromUri] string device)
        {
            EngineStopRegistryResponse response = new EngineStopRegistryResponse();

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
                response = (new EngineStopDao()).GetEngineStop(device);               

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
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(EngineStopResponse))]
        public EngineStopResponse ListEvents([FromUri] string search, [FromUri] int page)
        {
            EngineStopResponse response = new EngineStopResponse();

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
                try
                {
                    response = (new EngineStopDao()).GetDeviceEngineStop(hierarchy, search, page, 10);
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
        [Route(BasicRoute + ResourceName + "/count")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(EngineStopNumberPagesResponse))]
        public EngineStopNumberPagesResponse NumberOfPages([FromUri] string search)
        {
            EngineStopNumberPagesResponse response = new EngineStopNumberPagesResponse();

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
                try
                {
                    response = (new EngineStopDao()).GetNumberPages(hierarchy, search);
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
        [HttpPost]
        [Route(BasicRoute + ResourceName + "/execute")] 
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(SendEngineStopResponse))]
        public SendEngineStopResponse Execute([FromUri(Name = "device")] string device, [FromUri(Name = "status")] int status)
        {
            SendEngineStopResponse response = new SendEngineStopResponse();

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
                try
                {
                    response = (new EngineStopDao()).ExecuteEngineStop(hierarchy, username, device, status);
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
