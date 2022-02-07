using SpiderFleetWebAPI.Models.Request.Admin;
using SpiderFleetWebAPI.Models.Response.Admin;
using SpiderFleetWebAPI.Utils.Admin;
using SpiderFleetWebAPI.Utils.VerifyUser;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Admin
{
    public class CompanyAdminController : ApiController
    {
        private const string Tag = "Administracion de Compañias";
        private const string BasicRoute = "api/";
        private const string ResourceName = "admin/";

        [Authorize]
        [HttpPut]
        [Route(BasicRoute + ResourceName + "change/company/device")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(CompanyAdminResponse))]
        public CompanyAdminResponse UpdateChangeCompanyDevice(CompanyAdminRequest request)
        {
            CompanyAdminResponse response = new CompanyAdminResponse();
            try
            {

                if (!(request is CompanyAdminRequest))
                {
                    response.success = false;
                    response.messages.Add("Objeto de entrada invalido");
                    return response;
                }

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
                    response = (new CompanyAdminDao()).UpdateChangeCompanyDevice(request.Company, request.Device);
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
