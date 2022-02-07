using SpiderFleetWebAPI.Models.Response.Main.Combo;
using SpiderFleetWebAPI.Utils.Main.Combo;
using SpiderFleetWebAPI.Utils.User;
using SpiderFleetWebAPI.Utils.VerifyUser;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Main.Combo
{
    public class ComboController : ApiController
    {
        private const string Tag = "Combos de Pagina Principal";
        private const string BasicRoute = "api/";
        private const string ResourceName = "main";

        //[Authorize]
        //[HttpGet]
        //[Route(BasicRoute + ResourceName + "/empresas")]
        //[SwaggerOperation(Tags = new[] { Tag })]
        //[SwaggerResponse(HttpStatusCode.OK, Type = typeof(EmpresaResponse))]
        //public EmpresaResponse GetEmpresas()
        //{
        //    EmpresaResponse response = new EmpresaResponse();

        //    string username = string.Empty;
        //    try
        //    {
        //        username = (new VerifyUser()).verifyTokenUser(User);
        //    }
        //    catch (Exception ex)
        //    {
        //        response.success = false;
        //        response.messages.Add(ex.Message);
        //        return response;
        //    }

        //   response = (new ComboDao()).ReadEmpresas(username);

        //    return response;
        //}

        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName + "/subempresas")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(SubEmpresasResponse))]
        public SubEmpresasResponse SubEmpresas()
        {
            SubEmpresasResponse response = new SubEmpresasResponse();

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

            response = (new ComboDao()).ReadSubEmpresas(hierarchy);

            return response;
        }

        //[Authorize]
        //[HttpGet]
        //[Route(BasicRoute + ResourceName + "list/subempresas")]
        //[SwaggerOperation(Tags = new[] { Tag })]
        //[SwaggerResponse(HttpStatusCode.OK, Type = typeof(SubEmpresasResponse))]
        //public SubEmpresasResponse GetSubEmpresas()
        //{
        //    SubEmpresasResponse response = new SubEmpresasResponse();

        //    string hierarchy = string.Empty;

        //    try
        //    {
        //        string username = string.Empty;
        //        username = (new VerifyUser()).verifyTokenUser(User);
        //        hierarchy = (new UserDao()).ReadUserHierarchy(username);
        //    }
        //    catch (Exception ex)
        //    {
        //        response.success = false;
        //        response.messages.Add(ex.Message);
        //        return response;
        //    }

        //    response = (new ComboDao()).ReadSubEmpresas(hierarchy);

        //    return response;
        //}
    }
}
