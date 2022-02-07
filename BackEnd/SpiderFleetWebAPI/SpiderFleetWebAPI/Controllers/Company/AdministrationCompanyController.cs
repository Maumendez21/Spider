using SpiderFleetWebAPI.Models.Request.Company;
using SpiderFleetWebAPI.Models.Response.Company;
using SpiderFleetWebAPI.Utils.Company;
using SpiderFleetWebAPI.Utils.VerifyUser;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Company
{
    public class AdministrationCompanyController : ApiController
    {
        private const string Tag = "Activacion y Desactivacion de Empresas";
        private const string BasicRoute = "api/";
        private const string ResourceName = "administration/company";

        [Authorize]
        [HttpPut]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(AdministrationCompanyResponse))]
        public AdministrationCompanyResponse Update([FromBody] AdministrationCompanyRequest companyRequest)
        {
            AdministrationCompanyResponse response = new AdministrationCompanyResponse();
            try
            {
                if (!(companyRequest is AdministrationCompanyRequest))
                {
                    response.success = false;
                    response.messages.Add("Objeto de entrada invalido");
                    return response;
                }

                string username = string.Empty;
                try
                {
                    username = (new VerifyUser()).verifyTokenUser(User);
                }
                catch (Exception ex)
                {
                    response.success = false;
                    response.messages.Add(ex.Message);
                    return response;
                }

                CredencialSpiderFleet.Models.Company.AdministrationCompany company = new CredencialSpiderFleet.Models.Company.AdministrationCompany();
                company.Node = companyRequest.Node;
                company.Active = companyRequest.Active;

                try
                {
                    response = (new CompanyDao()).UpdateStatus(company);
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
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(AdministrationCompanyListResponse))]
        public AdministrationCompanyListResponse GetList()
        {
            AdministrationCompanyListResponse response = new AdministrationCompanyListResponse();

            string username = string.Empty;
            try
            {
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
                try
                {
                    response = (new CompanyDao()).ReadListCompanyNode();
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
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(AdministrationCompanyRegistryResponse))]
        public AdministrationCompanyRegistryResponse GetRegistryId([FromUri(Name = "id")] string id)
        {
            AdministrationCompanyRegistryResponse response = new AdministrationCompanyRegistryResponse();
            try
            {
                try
                {
                    response = (new CompanyDao()).ReadCompanyNode(id);
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
