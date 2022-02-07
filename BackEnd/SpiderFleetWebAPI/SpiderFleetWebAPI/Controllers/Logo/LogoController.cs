using CredencialSpiderFleet.Models.Useful;
using SpiderFleetWebAPI.Models.Response.Logo;
using SpiderFleetWebAPI.Utils.Logo;
using SpiderFleetWebAPI.Utils.User;
using SpiderFleetWebAPI.Utils.VerifyUser;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Logo
{
    public class LogoController : ApiController
    {
        private const string Tag = "Mantenimiento de Obds";
        private const string BasicRoute = "api/";
        private const string ResourceName = "changed/logo";


        [Authorize]
        [HttpPost]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(LogoResponse))]
        public LogoResponse Create()
        {
            LogoResponse response = new LogoResponse();

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
             
            if(!UseFul.IsPrincipal(hierarchy))
            {
                response.success = false;
                response.messages.Add("No tienes acceso a esta Función");
                return response;
            }

            try
            {
                try
                {

                    var fileCarga = HttpContext.Current.Request.Files.Count > 0 ? HttpContext.Current.Request.Files[0] : null;
                    if (fileCarga != null & fileCarga.ContentLength > 0)
                    {
                        response = (new LogoDao()).Create(fileCarga, UseFul.NumberEmpresa(hierarchy));
                    }
                    
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
        [Route(BasicRoute + ResourceName + "/image/update")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(LogoResponse))]
        public LogoResponse Update()
        {
            LogoResponse response = new LogoResponse();

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

            if (!UseFul.IsPrincipal(hierarchy))
            {
                response.success = false;
                response.messages.Add("No tienes acceso a esta Función");
                return response;
            }

            try
            {
                try
                {

                    var fileCarga = HttpContext.Current.Request.Files.Count > 0 ? HttpContext.Current.Request.Files[0] : null;
                    if (fileCarga != null & fileCarga.ContentLength > 0)
                    {
                        response = (new LogoDao()).Update(fileCarga, UseFul.NumberEmpresa(hierarchy));
                    }

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
