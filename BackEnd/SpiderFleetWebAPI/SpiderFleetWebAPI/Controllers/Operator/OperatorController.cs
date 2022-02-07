using SpiderFleetWebAPI.Models.Request.Operator;
using SpiderFleetWebAPI.Models.Response.Operator;
using SpiderFleetWebAPI.Utils.Operator;
using SpiderFleetWebAPI.Utils.VerifyUser;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Operator
{
    public class OperatorController : ApiController
    {
        private const string Tag = "Mantenimiento de Operadores";
        private const string BasicRoute = "api/";
        private const string ResourceName = "administration/operators";

        /// <summary>
        /// Alta de Usuario Inicial
        /// </summary>
        /// <remarks>
        /// Este EndPoint genera el registro del Usuario 
        /// #### Ejemplo de entrada
        /// { 
        /// "name" : "Gerardo",
        /// "lastname" : "Rodriguez Perez",
        /// "email" : "gerardo@gmail.com",
        /// "username" : "gerardorp",
        /// "password" : "123456",
        /// "telephone" : "960785"
        /// }
        /// </remarks>
        /// <example>
        /// ```
        /// {
        /// "name" : "string",
        /// "lastname" : ""string",
        /// "email" : "string",
        /// "username" : "string",
        /// "password" : "string",
        /// "telephone" : "string",
        /// }
        /// ```
        /// </example>
        /// <param name="operatorRequest">Objeto de entrada para el Endpoint</param>
        /// <returns>success = true o false;</returns>
        [Authorize]
        [HttpPost]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(OperatorResponse))]
        public OperatorResponse Create([FromBody] OperatorRequest operatorRequest)
        {
            OperatorResponse response = new OperatorResponse();

            try
            {
                if (!(operatorRequest is OperatorRequest))
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

                CredencialSpiderFleet.Models.Operator.Operator operators = new CredencialSpiderFleet.Models.Operator.Operator();
                operators.Device = operatorRequest.Device;
                operators.Name = operatorRequest.Name;
                operators.Address = operatorRequest.Address;
                operators.Email = operatorRequest.Email;
                operators.Location = operatorRequest.Location;
                operators.Position = operatorRequest.Position;
                operators.Telephone = operatorRequest.Telephone;
                operators.Status = 1;
                operators.IdImg = 0;

                try
                {
                    response = (new OperatorDao()).Create(operators);
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
        /// Actualiza de Compañia
        /// </summary>
        /// <remarks>
        /// Este EndPoint que actualiza el registro 
        /// #### Ejemplo de entrada
        /// ```
        /// {
        /// "name" : "Gerado Emilio",
        /// "lastname" : "Rodriguez Perez.",
        /// "email" : "gerardo@gmail.com",
        /// "username" : "gerardorp",
        /// "telephone" : "960785"
        /// }
        /// ```
        /// </remarks>
        /// <example>
        /// ```
        /// {
        /// "name" : "string",
        /// "lastname" : ""string",
        /// "email" : "string",
        /// "username" : "string",
        /// "telephone" : "string",
        /// }
        /// ```
        /// </example>
        /// <param name="userRequest">Objeto de entrada para el Endpoint</param>
        /// <returns>Es un objeto de User estructura similar a la tabla User</returns>
        [Authorize]
        [HttpPut]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(OperatorResponse))]
        public OperatorResponse Update([FromBody] OperatorUpdateRequest operatorRequest)
        {
            OperatorResponse response = new OperatorResponse();
            try
            {
                if (!(operatorRequest is OperatorUpdateRequest))
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

                CredencialSpiderFleet.Models.Operator.Operator operators = new CredencialSpiderFleet.Models.Operator.Operator();
                operators.Device = operatorRequest.Device;
                operators.Name = operatorRequest.Name;
                operators.Address = operatorRequest.Address;
                operators.Email = operatorRequest.Email;
                operators.Location = operatorRequest.Location;
                operators.Position = operatorRequest.Position;
                operators.Telephone = operatorRequest.Telephone;
                operators.Id = operatorRequest.Id;

                try
                {
                    response = (new OperatorDao()).Update(operators);
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
        [Route(BasicRoute + ResourceName + "/device/operator")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(OperatorResponse))]
        public OperatorResponse UpdateDeviceOperator([FromBody] OperatorUpdateDeviceOperator operatorRequest)
        {
            OperatorResponse response = new OperatorResponse();
            try
            {
                if (!(operatorRequest is OperatorUpdateDeviceOperator))
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

                CredencialSpiderFleet.Models.Operator.Operator operators = new CredencialSpiderFleet.Models.Operator.Operator();
                operators.Device = operatorRequest.Device;
                operators.Id = operatorRequest.Id;

                try
                {
                    response = (new OperatorDao()).UpdateDeviceOperador(operators);
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
        /// Obtiene todos los registros de User
        /// </summary>
        /// <remarks>
        /// Este EndPoint obtiene todos los registros de la Tabla User
        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName + "/list")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(OperatorListResponse))]
        public OperatorListResponse GetListAsync()
        {
            OperatorListResponse response = new OperatorListResponse();

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
                    response = (new OperatorDao()).Read(username);
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
        /// Obtiene un registro de User
        /// </summary>
        /// <remarks>
        /// Este EndPoint obtiene todos los registros de la Tabla User
        /// <param name="idusername" >Id del Usuario</param>
        /// <returns>Es un objeto de User </returns>
        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName + "/registry")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(OperatorRegistryResponse))]
        public OperatorRegistryResponse GetRegistryId([FromUri(Name = "id")] int id)
        {
            OperatorRegistryResponse response = new OperatorRegistryResponse();

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
                    response = (new OperatorDao()).ReadId(id);
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
        /// Obtiene un registro de User
        /// </summary>
        /// <remarks>
        /// Este EndPoint obtiene todos los registros de la Tabla User
        /// <param name="idusername" >Id del Usuario</param>
        /// <returns>Es un objeto de User </returns>
        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName + "/device")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(OperatorRegistryResponse))]
        public OperatorRegistryResponse GetRegistryDevice([FromUri(Name = "device")] string device)
        {
            OperatorRegistryResponse response = new OperatorRegistryResponse();

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
                    response = (new OperatorDao()).ReadDevice(device);
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
        /// Obtiene un registro de User
        /// </summary>
        /// <remarks>
        /// Este EndPoint obtiene todos los registros de la Tabla User
        /// <param name="idusername" >Id del Usuario</param>
        /// <returns>Es un objeto de User </returns>
        [Authorize]
        [HttpDelete]
        [Route(BasicRoute + ResourceName + "/device")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(OperatorResponse))]
        public OperatorResponse DeleteRegistry([FromUri(Name = "id")] int id)
        {
            OperatorResponse response = new OperatorResponse();

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
                    response = (new OperatorDao()).Delete(id);
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