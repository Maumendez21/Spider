using SpiderFleetWebAPI.Models.Request.Inventory.TypeDevice;
using SpiderFleetWebAPI.Models.Response.Inventory.TypeDevice;
using SpiderFleetWebAPI.Utils.Inventory.TypeDevice;
using SpiderFleetWebAPI.Utils.VerifyUser;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Net;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Inventory.TypeDevice
{
    public class TypeDeviceController : ApiController
    {
        //private UseFul use = new UseFul();
        private const string Tag = "Mantenimiento de Tipo de Dispositivos";
        private const string BasicRoute = "api/";
        private const string ResourceName = "inventario/typedevices";
        
        /// <summary>
        /// Alta de Tipo de Dispositivos
        /// </summary>
        /// <remarks>
        /// Este EndPoint nos genera el registro de la Tipo de Dispositivos
        /// #### Ejemplo de entrada
        /// ##### Inserta un registro En la Tabla Tipo de Dispositivos
        /// ```
        /// {
        /// "name":"Castel",
        /// "description":"xxx"
        /// }
        /// ```
        /// </remarks>
        /// <example>
        /// ```
        /// {
        /// "name" : "string",
        /// "description":"string"
        /// }
        /// ```
        /// </example>
        /// <param name="typeRequest">Objeto de entrada para el Endpoint</param>
        /// <returns>Es un objeto de Tipo de Dispositivos estructura similar a la tabla Tipo de Dispositivos</returns>
        [Authorize]
        [HttpPost]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(TypeDeviceResponse))]
        public TypeDeviceResponse Create([FromBody] TypeDeviceRequest typeRequest)
        {
            TypeDeviceResponse response = new TypeDeviceResponse();

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
                if (!(typeRequest is TypeDeviceRequest))
                {
                    response.success = false;
                    response.messages.Add("Objeto de entrada invalido");
                    return response;
                }

                CredencialSpiderFleet.Models.Inventory.TypeDevice.TypeDevices devices = new CredencialSpiderFleet.Models.Inventory.TypeDevice.TypeDevices();
                devices.Name = typeRequest.Name;
                devices.Description = typeRequest.Description;
                try
                {
                    response = (new TypeDeviceDao()).Create(devices);
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
        /// Actuliazacion de Tipo de Dispositivos
        /// </summary>
        /// <remarks>
        /// Este EndPoint que actualiza el registro de la tabla status
        /// #### Ejemplo de entrada
        /// ##### Actualiza un resgistro en la Tabla Tipo de Dispositivos
        /// ```
        /// {
        /// "idtypedevice":2,
        /// "name":"Castel",
        /// "description":"xxx"
        /// }
        /// ```
        /// </remarks>
        /// <example>
        /// ```
        /// {
        /// "idtypedevice":"int",
        /// "name" : "string",
        /// "description":"string"
        /// }
        /// ```
        /// </example>
        /// <param name="typeRequest">Objeto de entrada para el Endpoint</param>
        /// <returns>Es un objeto de Tipo de Dispositivos estructura similar a la tabla Tipo de Dispositivos</returns>
        [Authorize]
        [HttpPut]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(TypeDeviceResponse))]
        public TypeDeviceResponse Update([FromBody] TypeDeviceUpdateRequest typeRequest)
        {
            TypeDeviceResponse response = new TypeDeviceResponse();

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
                if (!(typeRequest is TypeDeviceUpdateRequest))
                {
                    response.success = false;
                    response.messages.Add("Objeto de entrada invalido");
                    return response;
                }

                CredencialSpiderFleet.Models.Inventory.TypeDevice.TypeDevices devices = new CredencialSpiderFleet.Models.Inventory.TypeDevice.TypeDevices();
                devices.idTypeDevice = typeRequest.idTypeDevice;
                devices.Name = typeRequest.Name;
                devices.Description = typeRequest.Description;
                try
                {
                    response = (new TypeDeviceDao()).Update(devices);
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
        /// Obtiene todos los registros de Tipo de Dispositivos
        /// </summary>
        /// <remarks>
        /// Este EndPoint obtiene todos los registros de la Tabla Tipo de Dispositivos
        /// <returns>Es una lista de Tipo de Dispositivos estructura similar a la tabla Tipo de Dispositivos</returns>
        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName + "/list")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(TypeDeviceResponse))]
        public TypeDeviceListResponse GetListAsync()
        {
            TypeDeviceListResponse response = new TypeDeviceListResponse();

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
                    response = (new TypeDeviceDao()).Read();
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
        /// Obtiene un registro por Id de la tabla Tipo de Dispositivos
        /// </summary>
        /// <remarks>
        /// Este EndPoint obtiene un registro
        /// <param name="id">Id del registro a consultar</param>
        /// <returns>Es un objeto de Tipo de Dispositivos estructura similar a la tabla Tipo de Dispositivos</returns>
        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName )]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(TypeDeviceRegistryResponse))]
        public TypeDeviceRegistryResponse GetListIdAsync([FromUri(Name = "id")]int id)
        {
            TypeDeviceRegistryResponse response = new TypeDeviceRegistryResponse();

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
                    response = (new TypeDeviceDao()).ReadId(id);
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

