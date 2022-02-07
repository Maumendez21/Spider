using SpiderFleetWebAPI.Models.Request.Sub.SubComapny;
using SpiderFleetWebAPI.Models.Response.Sub.SubComapny;
using SpiderFleetWebAPI.Utils.Sub.SubComapny;
using SpiderFleetWebAPI.Utils.VerifyUser;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Sub.SubCompany
{
    public class SubCompanyAssignmentUserController : ApiController
    {
        private const string Tag = "Mantenimiento de Asignacion de Usuarios a SubCompañias";
        private const string BasicRoute = "api/";
        private const string ResourceName = "administration/usersasissignment";

        /// <summary>
        /// Asignacion de Usuarios a Sub Grupos
        /// </summary>
        /// <remarks>
        /// Este EndPoint asigna usuarios a subempresas
        /// #### Ejemplo de entrada
        /// ##### Inserta un registro En la Tabla Obds
        /// ```
        /// {
        ///  "IdSubCompany": "",
        ///  "AssignmentUsers": ["", "", "", "", ""]
        /// }
        /// ```
        /// </remarks>
        /// <example>
        /// ```
        /// {
        ///  "IdSubCompany": "string",
        ///  "AssignmentUsers": ["string","string"]
        /// }
        /// ```
        /// </example>
        /// <param name="obdListRequest">Objeto de entrada para el Endpoint</param>
        /// <returns>true o false</returns>
        [Authorize]
        [HttpPut]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ListErrorSubCompanyResponse))]
        public ListErrorSubCompanyResponse Update([FromBody] SubCompanyAssignmentUserRequest obdListRequest)
        {
            ListErrorSubCompanyResponse response = new ListErrorSubCompanyResponse();
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

                if (!(obdListRequest is SubCompanyAssignmentUserRequest))
                {
                    response.success = false;
                    response.messages.Add("Objeto de entrada invalido");
                    return response;
                }

                CredencialSpiderFleet.Models.Sub.SubComapny.SubCompanyAssignmentUsers obdList = new CredencialSpiderFleet.Models.Sub.SubComapny.SubCompanyAssignmentUsers();
                obdList.IdSubCompany = obdListRequest.IdSubCompany;
                obdList.AssignmentUsers = obdListRequest.AssignmentUsers;

                try
                {
                    response = (new SubCompanyAssignmentUsersDao()).Update(obdList);
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

