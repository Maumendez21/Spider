using SpiderFleetWebAPI.Models.Request.Catalog.Mechanics;
using SpiderFleetWebAPI.Models.Response.Catalog.Mechanics;
using SpiderFleetWebAPI.Utils.Catalog.Mechanics;
using SpiderFleetWebAPI.Utils.User;
using SpiderFleetWebAPI.Utils.VerifyUser;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Catalog.Mechanics
{
    public class MechanicsController : ApiController
    {
        private const string Tag = "Mantenimiento de Tipos de Servicios";
        private const string BasicRoute = "api/";
        private const string ResourceName = "administration/mechanics";
        private CredencialSpiderFleet.Models.Useful.UseFul useful = new CredencialSpiderFleet.Models.Useful.UseFul();

        /// <summary>
        /// Alta de Mecanico
        /// </summary>
        /// <remarks>
        /// Este EndPoint nos genera un registro en Mechanics
        /// #### Ejemplo de entrada
        /// ##### Inserta un registro En la Tabla Mechanics
        /// ```
        /// {
        /// "FullName" : "Juanito perez",
        /// "Phone" : "2214538142",
        /// "Specialty" : "Barrer"
        /// }
        /// ```
        /// </remarks>
        /// <example>
        /// ```
        /// {
        /// "FullName" : "string"
        /// "Phone" : "string"
        /// "Specialty" : "string"
        /// }
        /// ```
        /// </example>
        /// <param name="mechanicRequest">Objeto de entrada para el Endpoint</param>
        /// <returns>Es un objeto de Mechanics estructura similar a la tabla Mecanicos</returns>
        [Authorize]
        [HttpPost]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(MechanicsResponse))]
        public MechanicsResponse Create([FromBody] MechanicRequest mechanicRequest)
        {
            MechanicsResponse response = new MechanicsResponse();

            try
            {
                if (!(mechanicRequest is MechanicRequest))
                {
                    response.success = false;
                    response.messages.Add("Objeto de entrada invalido");
                    return response;
                }

                string username = string.Empty;
                string hierarchy = string.Empty;

                try
                {
                    username = (new VerifyUser()).verifyTokenUser(User);
                    hierarchy = (new UserDao()).ReadUserHierarchy(username);
                }
                catch (Exception ex)
                {
                    response.success = false;
                    response.messages.Add(ex.Message);
                    return response;
                }

                CredencialSpiderFleet.Models.Catalogs.Mechanics.Mechanics mechanics = new CredencialSpiderFleet.Models.Catalogs.Mechanics.Mechanics();
                mechanics.FullName = mechanicRequest.FullName;
                mechanics.Node = hierarchy;
                mechanics.Phone = mechanicRequest.Phone;
                mechanics.Specialty = mechanicRequest.Specialty;

                try
                {
                    response = (new MechanicDao()).Create(mechanics);

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
        /// Actualiazacion de Mecanicos
        /// </summary>
        /// <remarks>
        /// Este EndPoint que actualiza el registro de la tabla Mechanics
        /// #### Ejemplo de entrada
        /// ##### Actualiza un resgistro en la Tabla Mechanics
        /// ```
        /// {
        /// "IdMechanic" : 2,
        /// "FullName" : "Juanito perez",
        /// "Phone" : "2213504738",
        /// "Specialty" : "Rines"
        /// }
        /// ```
        /// </remarks>
        /// <example>
        /// ```
        /// {
        /// "id" : "int",
        /// "FullName" : "string"
        /// "Phone" : "string"
        /// "Specialty" : "string"
        /// }
        /// ```
        /// </example>
        /// <param name="mechanicRequest">Objeto de entrada para el Endpoint</param>
        /// <returns>Es un objeto de Mechanics estructura similar a la tabla Mecanicos</returns>
        [Authorize]
        [HttpPut]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(MechanicsResponse))]
        public MechanicsResponse Update([FromBody] MechanicUpdateRequest mechanicRequest)
        {
            MechanicsResponse response = new MechanicsResponse();
            try
            {
                if (!(mechanicRequest is MechanicUpdateRequest))
                {
                    response.success = false;
                    response.messages.Add("Objeto de entrada invalido");
                    return response;
                }

                CredencialSpiderFleet.Models.Catalogs.Mechanics.Mechanics mechanics = new CredencialSpiderFleet.Models.Catalogs.Mechanics.Mechanics();
                mechanics.IdMechanics = mechanicRequest.IdMechanic;
                mechanics.FullName = mechanicRequest.FullName;
                mechanics.Phone = mechanicRequest.Phone;
                mechanics.Specialty = mechanicRequest.Specialty;

                try
                {
                    response = (new MechanicDao()).Update(mechanics);
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
        /// Lista de Mecanicos
        /// </summary>
        /// <remarks>
        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName + "/list")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(MechanicsListResponse))]
        public MechanicsListResponse GetListAsync()
        {
            MechanicsListResponse response = new MechanicsListResponse();
            try
            {
                string username = string.Empty;
                string hierarchy = string.Empty;

                try
                {
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
                    response = (new MechanicDao()).Read(hierarchy);
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
        /// Obtiene un registro por Id de la tabla Mechanics
        /// </summary>
        /// <remarks>
        /// Este EndPoint obtiene un registro
        /// #### Ejemplo de entrada
        /// ##### Regresa un registros de la tabla Mechanics
        /// <param name="id">Id del registro a consultar</param>
        /// <returns>Es un objeto de Mechanics estructura similar a la tabla de Mecanicos</returns>
        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(MechanicsRegistryResponse))]
        public MechanicsRegistryResponse GetListIdAsync([FromUri(Name = "id")] int id)
        {
            MechanicsRegistryResponse response = new MechanicsRegistryResponse();
            try
            {
                try
                {
                    response = (new MechanicDao()).ReadId(id);
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