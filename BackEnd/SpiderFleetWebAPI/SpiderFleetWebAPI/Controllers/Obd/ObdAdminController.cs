using SpiderFleetWebAPI.Models.Request.Obd;
using SpiderFleetWebAPI.Models.Response.Obd;
using SpiderFleetWebAPI.Utils.Obd;
using SpiderFleetWebAPI.Utils.User;
using SpiderFleetWebAPI.Utils.VerifyUser;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Net;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Obd
{
    public class ObdAdminController : ApiController
    {

        private const string Tag = "Informacion General de los Obds por Empresa";
        private const string BasicRoute = "api/";
        private const string ResourceName = "obd/admin/";

        /// <summary>
        /// Lista de dispositivos por empresa
        /// </summary>
        /// <remarks>
        /// Este EndPoint asigna usuarios a subempresas
        /// #### Ejemplo de entrada
        /// ##### Inserta un registro En la Tabla Obds
        /// ```
        /// {
        ///  "id": "72"
        /// }
        /// ```
        /// </remarks>
        /// <example>
        /// ```
        /// {
        ///  "id": "string"
        /// }
        /// ```
        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName + "list/general/status")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ObdAdminResponse))]
        public ObdAdminResponse GetId([FromUri(Name = "id")]string id)
        {
            ObdAdminResponse response = new ObdAdminResponse();
            try
            {
                string hierarchy = string.Empty;

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
                    response = (new ObdAdminDao()).ReadGeneralStatusDevice(id);
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
        /// Asignacion de Usuarios a Subempresas
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
        ///  "AssignmentUsers": ["string"]
        /// }
        /// ```
        /// </example>
        /// <param name="obdListRequest">Objeto de entrada para el Endpoint</param>
        /// <returns>true o false</returns>
        [Authorize]
        [HttpPut]
        [Route(BasicRoute + ResourceName + "obdsasissignment")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ListErrorObdResponse))]
        public ListErrorObdResponse Update([FromBody] SubCompanyAssignmentObdRequest obdListRequest)
        {
            ListErrorObdResponse response = new ListErrorObdResponse();
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

                if (!(obdListRequest is SubCompanyAssignmentObdRequest))
                {
                    response.success = false;
                    response.messages.Add("Objeto de entrada invalido");
                    return response;
                }

                CredencialSpiderFleet.Models.Obd.SubCompanyAssignmentObds obdList = new CredencialSpiderFleet.Models.Obd.SubCompanyAssignmentObds();
                obdList.IdSubCompany = obdListRequest.IdSubCompany;
                obdList.AssignmentObds = obdListRequest.AssignmentObds;

                try
                {
                    response = (new SubCompanyAssignmentObdsDao()).Update(obdList);
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
        /// Actualiza el nombre de y la empresa
        /// </summary>
        /// <remarks>
        /// Este EndPoint actualiza nombre y cambio de empresa
        /// #### Ejemplo de entrada
        /// ```
        /// {
        ///  "IdDevice": "213WP202020202020",
        ///  "Name": "",
        ///  "Hierarchy": "/"
        /// }
        /// ```
        /// </remarks>
        /// <example>
        /// ```
        /// {
        ///  "IdDevice": "string",
        ///  "Name": "string",
        ///  "Hierarchy": "string"
        /// }
        /// ```
        /// </example>
        /// <param name="companyRequest">Objeto de entrada para el Endpoint</param>
        /// <returns>false o true</returns>
        [Authorize]
        [HttpPut]
        [Route(BasicRoute + ResourceName + "obd")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ObdResponse))]
        public ObdResponse Update([FromBody] ObdAssignmentUpdateRequest request)
        {
            ObdResponse response = new ObdResponse();

            if (!(request is ObdAssignmentUpdateRequest))
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
                try
                {
                    CredencialSpiderFleet.Models.Obd.ObdAssignmentUpdate obd = new CredencialSpiderFleet.Models.Obd.ObdAssignmentUpdate();
                    obd.IdDevice = request.IdDevice;
                    obd.Hierarchy = request.Hierarchy;
                    obd.Name = request.Name;
                    obd.Responsable = request.Responsable;

                    response = (new SubCompanyAssignmentObdsDao()).Update(obd);
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
        /// Lista de obds por empresa
        /// </summary>
        /// <remarks>
        /// Este EndPoint devuelve una lista de dispositivos por empresa seleccionada
        /// #### Ejemplo de entrada
        /// ```
        /// {
        ///  "Hierarchy": "/"
        /// }
        /// ```
        /// </remarks>
        /// <example>
        /// ```
        /// {
        ///  "Hierarchy": "string"
        /// }
        /// ```
        /// </example>
        /// <param name="companyRequest">Objeto de entrada para el Endpoint</param>
        /// <returns>false o true</returns>
        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName + "configuration/list/obd")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ListObdResponse))]
        public ListObdResponse ListDeviceByUserConfig()
        {
            ListObdResponse response = new ListObdResponse();

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
                try
                {
                    //int pagina = 1;
                    //int registros = 10;
                    //int respuesta = registros * (pagina - 1);

                    response = (new SubCompanyAssignmentObdsDao()).ListDeviceHierarchy(hierarchy);
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
        /// Lista de obds por empresa
        /// </summary>
        /// <remarks>
        /// Este EndPoint devuelve una lista de dispositivos por empresa seleccionada
        /// #### Ejemplo de entrada
        /// ```
        /// {
        ///  "Hierarchy": "/"
        /// }
        /// ```
        /// </remarks>
        /// <example>
        /// ```
        /// {
        ///  "Hierarchy": "string"
        /// }
        /// ```
        /// </example>
        /// <param name="companyRequest">Objeto de entrada para el Endpoint</param>
        /// <returns>false o true</returns>
        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName + "manager/list/obd")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ListObdResponse))]
        public ListObdResponse ListDeviceByUserAdmin() 
        {
            ListObdResponse response = new ListObdResponse();

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
                try
                {
                    response = (new SubCompanyAssignmentObdsDao()).ListDeviceHierarchy(hierarchy);
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
