using SpiderFleetWebAPI.Models.Request.Catalog.Version;
using SpiderFleetWebAPI.Models.Request.Permission;
using SpiderFleetWebAPI.Models.Response.Permission;
using SpiderFleetWebAPI.Utils.Permission;
using SpiderFleetWebAPI.Utils.VerifyUser;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Permission
{
    public class PermissionController : ApiController
    {

        private const string Tag = "Mantenimiento de Permisos";
        private const string BasicRoute = "api/";
        private const string ResourceName = "configuration/permission";

        [Authorize]
        [HttpPost]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(PermissionResponse))]
        public PermissionResponse CreateOrUpdatePermission(PermissionDataRequest request)
        {
            PermissionResponse response = new PermissionResponse();
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
                    CredencialSpiderFleet.Models.Permission.ListPermission permission = new CredencialSpiderFleet.Models.Permission.ListPermission();

                    permission.PermissionList = request.PermissionList;

                    response = (new PermissionDao()).CreateorUpdate(permission);
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
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ModulesResponse))]
        public ModulesResponse GetListAsync(string user)
        {
            ModulesResponse response = new ModulesResponse();
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
                    response = (new PermissionDao()).Read(user);
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