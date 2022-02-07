using SpiderFleetWebAPI.Models.Request.Obd;
using SpiderFleetWebAPI.Models.Response.Obd;
using SpiderFleetWebAPI.Utils.Obds;
using SpiderFleetWebAPI.Utils.User;
using SpiderFleetWebAPI.Utils.VerifyUser;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Net;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Obd
{
    public class ObdsController : ApiController
    {
        private const string Tag = "Mantenimiento de Obds";
        private const string BasicRoute = "api/";
        private const string ResourceName = "management/obd";

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
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ObdResponse))]
        public ObdResponse Create([FromBody] ObdRequest request)
        {
            ObdResponse response = new ObdResponse();

            if (!(request is ObdRequest))
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
                    CredencialSpiderFleet.Models.Obd.Obd obd = new CredencialSpiderFleet.Models.Obd.Obd();
                    obd.IdDevice = request.IdDevice;
                    obd.Label = request.Label;
                    obd.Name = request.Name;
                    obd.Hierarchy = request.Hierarchy;
                    obd.IdType = request.IdType;
                    obd.IdSim = request.IdSim;
                    obd.Status = 1;
                    obd.Motor = request.Motor;
                    obd.Panico = request.Panico;

                    response = (new ObdDao()).Create(obd);
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
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ObdResponse))]
        public ObdResponse Update([FromBody] ObdUpdateRequest request)
        {
            ObdResponse response = new ObdResponse();

            if (!(request is ObdUpdateRequest))
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
                    CredencialSpiderFleet.Models.Obd.ObdUpdate obd = new CredencialSpiderFleet.Models.Obd.ObdUpdate();
                    obd.IdDevice = request.IdDevice;
                    obd.IdDeviceAnt = request.IdDeviceAnt;
                    obd.Label = request.Label;
                    obd.Hierarchy = request.Hierarchy;
                    obd.IdType = request.IdType;
                    obd.IdSim = request.IdSim;
                    obd.Status = request.Status;
                    obd.Motor = request.Motor;
                    obd.Panico = request.Panico;
                    obd.Name = request.Name;

                    response = (new ObdDao()).Update(obd);
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
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ObdListResponse))]
        public ObdListResponse GetList()
        {
            ObdListResponse response = new ObdListResponse();

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
                    response = (new ObdDao()).Read(hierarchy);
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
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ObdRegistryResponse))]
        public ObdRegistryResponse GetId([FromUri(Name = "id")]string id)
        {
            ObdRegistryResponse response = new ObdRegistryResponse();
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
                    response = (new ObdDao()).ReadId(hierarchy, id);
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
