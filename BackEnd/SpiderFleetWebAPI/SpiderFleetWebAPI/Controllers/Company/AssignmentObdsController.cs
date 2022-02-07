using SpiderFleetWebAPI.Models.Response.Company;
using SpiderFleetWebAPI.Utils.Company;
using SpiderFleetWebAPI.Utils.VerifyUser;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Company
{
    public class AssignmentObdsController : ApiController
    {
        private const string Tag = "Obds Asignados a Usuario";
        private const string BasicRoute = "api/";
        private const string ResourceName = "clientes/obds/asignados";


        /// <summary>
        /// Obtiene todos los registros de Compañia
        /// </summary>
        /// <remarks>
        /// Este EndPoint obtiene todos los registros de la Tabla Compañia
        /// <returns>Es una lista de Compañia estructura similar a la tabla Compañia</returns>
        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName + "/list")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(AssignmentObdsResponse))]
        public AssignmentObdsResponse GetListAsync()
        {
            AssignmentObdsResponse response = new AssignmentObdsResponse();
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

                try
                {
                    response = (new AssignmentObdsDao()).Read(username);
                   
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
