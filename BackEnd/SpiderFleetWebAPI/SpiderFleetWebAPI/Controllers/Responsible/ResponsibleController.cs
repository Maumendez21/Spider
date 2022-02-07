using SpiderFleetWebAPI.Models.Request.Responsible;
using SpiderFleetWebAPI.Models.Response.Responsible;
using SpiderFleetWebAPI.Utils.Responsible;
using SpiderFleetWebAPI.Utils.User;
using SpiderFleetWebAPI.Utils.VerifyUser;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Responsible
{
    public class ResponsibleController : ApiController
    {
        private const string Tag = "Mantenimiento de Responsables";
        private const string BasicRoute = "api/";
        private const string ResourceName = "configuration/responsible";

        /// <summary>
        /// Alta de Dispositivos
        /// </summary>
        /// <remarks>
        /// Este EndPoint nos genera el registro de la SubCompañia o Sub Grupo
        /// #### Ejemplo de entrada
        /// ```
        /// {
        ///  "IdDevice": "213WP202020202020",
        ///  "Name": "string",
        ///  "Label": "string",
        ///  "Hierarchy": "string",
        ///  "IdType": 0,
        ///  "IdSim": 0,
        ///  "Motor": 0,
        ///  "Panico": 0
        /// }
        /// ```
        /// </remarks>
        /// <example>
        /// ```
        /// {
        ///  "IdDevice": "string",
        ///  "Name": "",
        ///  "Label": "Label 2020",
        ///  "Hierarchy": "string",
        ///  "IdType": 0,
        ///  "IdSim": 0,
        ///  "Motor": 0,
        ///  "Panico": 0
        /// }
        /// ```
        /// </example>
        /// <param name="companyRequest">Objeto de entrada para el Endpoint</param>
        /// <returns>false o true</returns>
        [Authorize]
        [HttpPost]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ResponsibleResponse))]
        public ResponsibleResponse Create([FromBody] ResponsibleRegistryRequest request)
        {
            ResponsibleResponse response = new ResponsibleResponse();
            string hierarchy = string.Empty;

            if (!(request is ResponsibleRegistryRequest))
            {
                response.success = false;
                response.messages.Add("Objeto de entrada invalido");
                return response;
            }

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
                    CredencialSpiderFleet.Models.Responsible.Responsible responsible = new CredencialSpiderFleet.Models.Responsible.Responsible();
                    responsible.Name = request.Name;
                    responsible.Email = request.Email;
                    responsible.Phone = request.Phone;
                    responsible.Hierarchy = hierarchy;
                    responsible.Area = request.Area;
                    responsible.Status = 1;

                    response = (new ResponsibleDao()).Create(responsible);
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
        /// Update de Dspositivos
        /// </summary>
        /// <remarks>
        /// Este EndPoint nos genera el registro de la SubCompañia o Sub Grupo
        /// #### Ejemplo de entrada
        /// ```
        /// {
        ///  "IdDevice": "213WP202020202020",
        ///  "IdDeviceAnt": "",
        ///  "Name": "",
        ///  "Label": "Label 2020",
        ///  "Hierarchy": "/",
        ///  "IdType": 1,
        ///  "IdSim": 1036,
        ///  "Motor": 0,
        ///  "Panico": 0,
        ///  "Status": 0
        /// }
        /// ```
        /// </remarks>
        /// <example>
        /// ```
        /// {
        ///  "IdDevice": "string",
        ///  "IdDeviceAnt": "string",
        ///  "Name": "string",
        ///  "Label": "string",
        ///  "Hierarchy": "string",
        ///  "IdType": 0,
        ///  "IdSim": 0,
        ///  "Motor": 0,
        ///  "Panico": 0,
        ///  "Status": 0
        /// }
        /// ```
        /// </example>
        /// <param name="companyRequest">Objeto de entrada para el Endpoint</param>
        /// <returns>false o true</returns>
        [Authorize]
        [HttpPut]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ResponsibleResponse))]
        public ResponsibleResponse Update([FromBody] ResponsibleUpdateRequest request)
        {
            ResponsibleResponse response = new ResponsibleResponse();
            string hierarchy = string.Empty;

            if (!(request is ResponsibleUpdateRequest))
            {
                response.success = false;
                response.messages.Add("Objeto de entrada invalido");
                return response;
            }

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
                    CredencialSpiderFleet.Models.Responsible.ResponsibleUpdate responsible = new CredencialSpiderFleet.Models.Responsible.ResponsibleUpdate();
                    responsible.Id = request.Id;
                    responsible.Name = request.Name;
                    responsible.Email = request.Email;
                    responsible.Phone = request.Phone;
                    responsible.Hierarchy = hierarchy;
                    responsible.Area = request.Area;

                    response = (new ResponsibleDao()).Update(responsible);
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
        /// Obtiene la lista de Obds
        /// </summary>
        /// <returns>Regresa lista de obds</returns>
        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName + "/list")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ResponsibleListResponse))]
        public ResponsibleListResponse GetList([FromUri(Name = "search")] string search)
        {
            ResponsibleListResponse response = new ResponsibleListResponse();

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
                    response = (new ResponsibleDao()).Read(hierarchy, search);
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
        /// Obtiene un Obd por Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Regresa un solo Registro</returns>
        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ResponsibleRegistryResponse))]
        public ResponsibleRegistryResponse GetId([FromUri(Name = "id")] string id)
        {
            ResponsibleRegistryResponse response = new ResponsibleRegistryResponse();
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
                    response = (new ResponsibleDao()).ReadId(hierarchy, id);
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
        [HttpDelete]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ResponsibleResponse))]
        public ResponsibleResponse DeleteId([FromUri(Name = "id")] string id)
        {
            ResponsibleResponse response = new ResponsibleResponse();
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
                    response = (new ResponsibleDao()).Delete(id, hierarchy);
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
        [Route(BasicRoute + ResourceName + "/device")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ResponsibleRegistryResponse))]
        public ResponsibleRegistryResponse GetDevice([FromUri(Name = "device")] string device)
        {
            ResponsibleRegistryResponse response = new ResponsibleRegistryResponse();
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
                    response = (new ResponsibleDao()).ReadDevice(device);
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
