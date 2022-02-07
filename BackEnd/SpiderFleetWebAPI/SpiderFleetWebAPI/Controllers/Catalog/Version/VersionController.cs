using SpiderFleetWebAPI.Models.Response.Catalog.Version;
using CredencialSpiderFleet.Models.Catalogs.Version;
using SpiderFleetWebAPI.Utils.Catalog.Version;
using SpiderFleetWebAPI.Utils.VerifyUser;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Net;
using System.Web.Http;
using SpiderFleetWebAPI.Models.Request.Catalog.Version;

namespace SpiderFleetWebAPI.Controllers.Catalog.Version
{
    public class VersionController : ApiController
    {
        private const string Tag = "Mantenimiento de Versiones de Vehiculos";
        private const string BasicRoute = "api/";
        private const string ResourceName = "administration/versions";

        /// <summary>
        /// Agregar versión
        /// </summary>
        /// <remarks>
        /// 
        [Authorize]
        [HttpPost]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(VersionListResponse))]
        public VersionDataResponse CreateVersionData(VersionDataRequest request)
        {
            VersionDataResponse response = new VersionDataResponse();
            try
            {
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

                try
                {
                    CredencialSpiderFleet.Models.Catalogs.Version.Version version = new CredencialSpiderFleet.Models.Catalogs.Version.Version();
                    version.Description = request.Description;

                    response = (new VersionDao()).CreateVersion(version);
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
        /// Lista de Versiones de Autos
        /// </summary>
        /// <remarks>
        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName + "/list")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(VersionListResponse))]
        public VersionListResponse GetListAsync()
        {
            VersionListResponse response = new VersionListResponse();
            try
            {
                string username = string.Empty;
                //string hierarchy = string.Empty;

                try
                {
                    username = (new VerifyUser()).verifyTokenUser(User);
                    //hierarchy = (new UserDao()).ReadUserHierarchy(username);
                }
                catch (Exception ex)
                {
                    response.success = false;
                    response.messages.Add(ex.Message);
                    return response;
                }

                try
                {
                    response = (new VersionDao()).Read();
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
