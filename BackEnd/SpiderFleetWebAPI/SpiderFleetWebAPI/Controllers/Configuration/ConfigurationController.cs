using SpiderFleetWebAPI.Models.Request.Configuration;
using SpiderFleetWebAPI.Models.Response.General;
using SpiderFleetWebAPI.Utils.Configuration;
using SpiderFleetWebAPI.Utils.User;
using SpiderFleetWebAPI.Utils.VerifyUser;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Configuration
{
    public class ConfigurationController : ApiController
    {
        private const string Tag = "Configuracion General por Empresa";
        private const string BasicRoute = "api/";
        private const string ResourceName = "configuration";


        [Authorize]
        [HttpPut]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ConfigurationResponse))]
        public ConfigurationResponse Update([FromBody] ConfigurationRequest configurationRequest)
        {
            ConfigurationResponse response = new ConfigurationResponse();

            try
            {
                if (!(configurationRequest is ConfigurationRequest))
                {
                    response.success = false;
                    response.messages.Add("Objeto de entrada invalido");
                    return response;
                }

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

                CredencialSpiderFleet.Models.Configuration.Configuration routes = new CredencialSpiderFleet.Models.Configuration.Configuration();
                routes.Id = configurationRequest.Id;
                routes.Value = configurationRequest.Value;

                try
                {
                    response = (new ConfigurationDao()).Update(routes);
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
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ConfigurationListResponse))]
        public ConfigurationListResponse GetRoutes()
        {
            ConfigurationListResponse response = new ConfigurationListResponse();

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
                response = (new ConfigurationDao()).Read(hierarchy);
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
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ConfigurationRegistryResponse))]
        public ConfigurationRegistryResponse GetIdRoute([FromUri(Name = "id")] int id)
        {
            ConfigurationRegistryResponse response = new ConfigurationRegistryResponse();

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
                response = (new ConfigurationDao()).Read(id);
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
