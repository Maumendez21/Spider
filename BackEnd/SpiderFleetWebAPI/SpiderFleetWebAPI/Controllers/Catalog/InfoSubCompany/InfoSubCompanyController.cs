using SpiderFleetWebAPI.Models.Response.Sub.SubComapny;
using SpiderFleetWebAPI.Utils.Catalog.InfoSubCompany;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Catalog.InfoSubCompany
{
    public class InfoSubCompanyController : ApiController
    {
        private const string Tag = "Listado de Combos de Subcompañias y Dispositivos que pertenecen";
        private const string BasicRoute = "api/";
        private const string ResourceName = "administration/subcompanies/assignament";

        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName + "/list")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(SubCompanyListByUserResponse))]
        public SubCompanyListByUserResponse GetListAsync()
        {
            SubCompanyListByUserResponse response = new SubCompanyListByUserResponse();
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                var claim = identity.Claims.ToList();
                var username = claim?.FirstOrDefault(x => x.Type.Equals("username", StringComparison.OrdinalIgnoreCase))?.Value;

                if (string.IsNullOrEmpty(username))
                {
                    response.success = false;
                    response.messages.Add("No contiene el UserName");
                    return response;
                }

                try
                {
                    response.listSubCompany = (new InfoSubCompanyDao()).Read(username);
                    response.success = true;
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

        //[Authorize]
        //[HttpGet]
        //[Route(BasicRoute + ResourceName)]
        //[SwaggerOperation(Tags = new[] { Tag })]
        //[SwaggerResponse(HttpStatusCode.OK, Type = typeof(ObdInfoUserListResponse))]
        //public ObdInfoUserListResponse GetListIdAsync([FromUri(Name = "id")]int id)
        //{
        //    ObdInfoUserListResponse response = new ObdInfoUserListResponse();
        //    try
        //    {
        //        var identity = (ClaimsIdentity)User.Identity;
        //        var claim = identity.Claims.ToList();
        //        var username = claim?.FirstOrDefault(x => x.Type.Equals("username", StringComparison.OrdinalIgnoreCase))?.Value;

        //        if (string.IsNullOrEmpty(username))
        //        {
        //            response.success = true;
        //            response.messages.Add("No contiene el UserName");
        //            return response;
        //        }

        //        if (id == 0)
        //        {
        //            response.success = true;
        //            response.messages.Add("El parametro es incorrecto");
        //            return response;
        //        }

        //        try
        //        {
        //            response.listObdInfo = (new InfoSubCompanyDao()).ReadId(id);
        //            response.success = true;
        //        }
        //        catch (Exception ex)
        //        {
        //            response.success = false;
        //            response.messages.Add(ex.Message);
        //            return response;
        //        }

        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        response.success = false;
        //        response.messages.Add(ex.Message);
        //        return response;
        //    }
        //}

        //[Authorize]
        //[HttpGet]
        //[Route(BasicRoute + ResourceName + "/info")]
        //[SwaggerOperation(Tags = new[] { Tag })]
        //[SwaggerResponse(HttpStatusCode.OK, Type = typeof(ObdInfoUserListResponse))]
        //public ObdInfoUserListResponse GetInfoId([FromUri(Name = "id")]int id)
        //{
        //    ObdInfoUserListResponse response = new ObdInfoUserListResponse();
        //    try
        //    {
        //        var identity = (ClaimsIdentity)User.Identity;
        //        var claim = identity.Claims.ToList();
        //        var username = claim?.FirstOrDefault(x => x.Type.Equals("username", StringComparison.OrdinalIgnoreCase))?.Value;

        //        if (string.IsNullOrEmpty(username))
        //        {
        //            response.success = true;
        //            response.messages.Add("No contiene el UserName");
        //            return response;
        //        }

        //        if (id == 0)
        //        {
        //            response.success = true;
        //            response.messages.Add("El parametro es incorrecto");
        //            return response;
        //        }

        //        try
        //        {
        //            response.listObdInfo = (new InfoSubCompanyDao()).ReadId(id);
        //            response.success = true;
        //        }
        //        catch (Exception ex)
        //        {
        //            response.success = false;
        //            response.messages.Add(ex.Message);
        //            return response;
        //        }

        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        response.success = false;
        //        response.messages.Add(ex.Message);
        //        return response;
        //    }
        //}
   

    }
}
