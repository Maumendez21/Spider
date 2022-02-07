using SpiderFleetWebAPI.Models.Request.Sub.SubComapny;
using SpiderFleetWebAPI.Models.Response.Main.Combo;
using SpiderFleetWebAPI.Models.Response.Sub.SubComapny;
using SpiderFleetWebAPI.Utils.Main.Combo;
using SpiderFleetWebAPI.Utils.Sub.SubComapny;
using SpiderFleetWebAPI.Utils.User;
using SpiderFleetWebAPI.Utils.VerifyUser;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Net;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.SubEmpresas
{
    public class SubEmpresasController : ApiController
    {
        private const string Tag = "Listado de Ultima posicion de Dispositivos";
        private const string BasicRoute = "api/";
        private const string ResourceName = "list/data";

        [Authorize]
        [HttpPost]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(SubCompanyResponse))]
        public SubCompanyResponse CreateSubEmpresas(SubCompanyRequest request)
        {
            SubCompanyResponse response = new SubCompanyResponse();

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
                    CredencialSpiderFleet.Models.Sub.SubComapny.SubCompany subCompany = new CredencialSpiderFleet.Models.Sub.SubComapny.SubCompany();
                    subCompany.UserName = username;
                    subCompany.IdFather = request.IdFather;
                    subCompany.NameSubCompany = request.NameSubCompany;

                    response = (new SubCompanyDao()).Create(subCompany);
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
        [HttpPut]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(SubCompanyResponse))]
        public SubCompanyResponse UpdateSubEmpresas(SubCompanyUpdateRequest request)
        {
            SubCompanyResponse response = new SubCompanyResponse();

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
                    CredencialSpiderFleet.Models.Sub.SubComapny.SubCompanyUpdate subCompany = new CredencialSpiderFleet.Models.Sub.SubComapny.SubCompanyUpdate();
                    subCompany.UserName = username;
                    subCompany.IdSubCompany = request.IdSubCompany;
                    subCompany.NameSubCompany = request.Name;

                    response = (new SubCompanyDao()).Update(subCompany);
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
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(SubEmpresasResponse))]
        public SubEmpresasResponse GetListLastPositionDevice()
        {
            SubEmpresasResponse response = new SubEmpresasResponse();

            try
            {
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
                    response = (new ComboDao()).ReadSubEmpresas(hierarchy);
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
