using SpiderFleetWebAPI.Models.Request.Sub.SubComapny;
using SpiderFleetWebAPI.Models.Response.Sub.SubComapny;
using SpiderFleetWebAPI.Utils.Sub.SubComapny;
using SpiderFleetWebAPI.Utils.VerifyUser;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Net;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Sub.SubCompany
{
    public class SubCompaniesController : ApiController
    {
        //private UseFul use = new UseFul();
        private const string Tag = "Mantenimiento de SubCompañias";
        private const string BasicRoute = "api/";
        private const string ResourceName = "administration/subcompanies";
        
        /// <summary>
        /// Alta de Grupo o Sub Grupo
        /// </summary>
        /// <remarks>
        /// Este EndPoint nos genera el registro de la SubCompañia o Sub Grupo
        /// #### Ejemplo de entrada
        /// ```
        /// {
        /// "IdFather" : "/72/",
        /// "NameSubCompany" : "Ejecutivo",
        /// }
        /// ```
        /// </remarks>
        /// <example>
        /// ```
        /// {
        /// "IdFather" : "string",
        /// "NameSubCompany" : "string",
        /// }
        /// ```
        /// </example>
        /// <param name="companyRequest">Objeto de entrada para el Endpoint</param>
        /// <returns>false o true</returns>
        [Authorize]
        [HttpPost]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(SubCompanyResponse))]
        public SubCompanyResponse Create([FromBody] SubCompanyRequest companyRequest)
        {
            SubCompanyResponse response = new SubCompanyResponse();

            try
            {
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

                if (!(companyRequest is SubCompanyRequest))
                {
                    response.success = false;
                    response.messages.Add("Objeto de entrada invalido");
                    return response;
                }

                CredencialSpiderFleet.Models.Sub.SubComapny.SubCompany company = new CredencialSpiderFleet.Models.Sub.SubComapny.SubCompany();
                company.UserName = username;
                company.IdFather = companyRequest.IdFather;
                company.NameSubCompany = companyRequest.NameSubCompany;
                
                try
                {
                    response = (new SubCompanyDao().Create(company));
                    
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
        /// Actualizacion de SubCompañiao Sub Grupo
        /// </summary>
        /// <remarks>
        /// Este EndPoint nos genera el registro de la SubCompañiao Sub Grupo
        /// #### Ejemplo de entrada
        /// ```
        /// {
        /// "idSubCompany" : /71/3/,
        /// "name" : "Cemex",
        /// }
        /// ```
        /// </remarks>
        /// <example>
        /// ```
        /// {
        /// "idSubCompany" : "string"
        /// "name" : "string",
        /// }
        /// ```
        /// </example>
        /// <param name="companyRequest">Objeto de entrada para el Endpoint</param>
        /// <returns>false o true</returns>
        [Authorize]
        [HttpPut]
        [Route(BasicRoute + ResourceName )]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(SubCompanyResponse))]
        public SubCompanyResponse Update([FromBody] SubCompanyUpdateRequest companyRequest)
        {
            SubCompanyResponse response = new SubCompanyResponse();

            try
            {
                if (!(companyRequest is SubCompanyUpdateRequest))
                {
                    response.success = false;
                    response.messages.Add("Objeto de entrada invalido");
                    return response;
                }

                CredencialSpiderFleet.Models.Sub.SubComapny.SubCompanyUpdate company = new CredencialSpiderFleet.Models.Sub.SubComapny.SubCompanyUpdate();
                company.IdSubCompany = companyRequest.IdSubCompany;
                company.NameSubCompany = companyRequest.Name;

                try
                {
                    response = (new SubCompanyDao().Update(company));
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
        /// Obtiene todos los registros de SubCompañia o Sub Grupo
        /// </summary>
        /// <remarks>
        /// Este EndPoint obtiene todos los registros de la Tabla SubCompañiao Sub Grupo
        /// <returns>Una lista de SubCompañia que estan asiciados los grupos que se encuentran bajo su jerarquia</returns>
        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName + "/list")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(SubCompanyResponse))]
        public SubCompanyListResponse GetListAsync()
        {
            SubCompanyListResponse response = new SubCompanyListResponse();

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
                    response = (new SubCompanyDao()).Read(username);
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
        /// Obtiene un registro por Id
        /// </summary>
        /// <remarks>
        /// Este EndPoint obtiene un registro
        /// <param name="id">Id del registro a consultar</param>
        /// <returns>Devuelve un registros de SubCompañia o Sub Grupo</returns>
        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName )]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(SubCompanyRegistryResponse))]
        public SubCompanyRegistryResponse GetListIdAsync([FromUri(Name = "id")]string id)
        {
            SubCompanyRegistryResponse response = new SubCompanyRegistryResponse();
            
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
                    response = (new SubCompanyDao()).ReadId(id);
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
