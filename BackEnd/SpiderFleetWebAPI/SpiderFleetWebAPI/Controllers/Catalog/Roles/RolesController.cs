using SpiderFleetWebAPI.Models.Request.Catalog.Roles;
using SpiderFleetWebAPI.Models.Response.Catalog.Roles;
using SpiderFleetWebAPI.Utils.Catalog.Roles;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Net;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Catalog.Roles
{
    public class RolesController : ApiController
    {
        private const string Tag = "Mantenimiento de Roles";
        private const string BasicRoute = "api/";
        private const string ResourceName = "administration/roles";
        private CredencialSpiderFleet.Models.Useful.UseFul useful = new CredencialSpiderFleet.Models.Useful.UseFul();

        /// <summary>
        /// Alta de Roles
        /// </summary>
        /// <remarks>
        /// Este EndPoint nos genera un registro en Roles
        /// #### Ejemplo de entrada
        /// ##### Inserta un registro En la Tabla Roles
        /// ```
        /// {
        /// "description" : "administrador"
        /// }
        /// ```
        /// </remarks>
        /// <example>
        /// ```
        /// {
        /// "description" : "string"
        /// }
        /// ```
        /// </example>
        /// <param name="rolRequest">Objeto de entrada para el Endpoint</param>
        /// <returns>Es un objeto de Roles estructura similar a la tabla Roles</returns>
        [Authorize]
        [HttpPost]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(RolesResponse))]
        public RolesResponse Create([FromBody] RolesRequest rolRequest)
        {
            RolesResponse response = new RolesResponse();

            try
            {
                if (!(rolRequest is RolesRequest))
                {
                    response.success = false;
                    response.messages.Add("Objeto de entrada invalido");
                    return response;
                }

                CredencialSpiderFleet.Models.Catalogs.Roles.Roles roles = new CredencialSpiderFleet.Models.Catalogs.Roles.Roles();
                roles.Description = rolRequest.Description;
                roles.Status = 1;
                try
                {
                    response = (new RolesDao()).Create(roles);
                   
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
        /// Actuliazacion de Roles
        /// </summary>
        /// <remarks>
        /// Este EndPoint que actualiza el registro de la tabla roles
        /// #### Ejemplo de entrada
        /// ##### Actualiza un resgistro en la Tabla Roles
        /// ```
        /// {
        /// "idrole":1,
        /// "description":"administradorssss"
        /// }
        /// ```
        /// </remarks>
        /// <example>
        /// ```
        /// {
        /// "idrole" : "int",
        /// "description":"string"
        /// }
        /// ```
        /// </example>
        /// <param name="rolRequest">Objeto de entrada para el Endpoint</param>
        /// <returns>Es un objeto de Roles estructura similar a la tabla Roles</returns>
        [Authorize]
        [HttpPut]
        [Route(BasicRoute + ResourceName )]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(RolesResponse))]
        public RolesResponse Update([FromBody] RolesUpdateRequest rolRequest)
        {
            RolesResponse response = new RolesResponse();
            try
            {
                if (!(rolRequest is RolesRequest))
                {
                    response.success = false;
                    response.messages.Add("Objeto de entrada invalido");
                    return response;
                }

                CredencialSpiderFleet.Models.Catalogs.Roles.Roles roles = new CredencialSpiderFleet.Models.Catalogs.Roles.Roles();
                roles.IdRole = rolRequest.IdRole;
                roles.Description = rolRequest.Description;

                try
                {
                    response = (new RolesDao()).Update(roles);
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
        /// Obtiene todos los registros de Roles
        /// </summary>
        /// <remarks>
        /// Este EndPoint obtiene todos los registros de la Tabla Roles
        /// ##### Regresa los registros de la tabla Roles
        /// <returns>Es una lista de Roles estructura similar a la tabla Roles</returns>
        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName + "/list")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(RolesResponse))]
        public RolesListResponse GetListAsync()
        {
            RolesListResponse response = new RolesListResponse();
            try
            {
                try
                {
                    response = (new RolesDao()).Read(0);
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
        /// Obtiene  los registros de Roles con status 1
        /// </summary>
        /// <remarks>
        /// ##### Regresa los registros de la tabla Roles
        /// <returns>Es una lista de Roles estructura similar a la tabla Roles</returns>
        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName + "/list/roles")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(RolesResponse))]
        public RolesListResponse GetListCatalagoAsync()
        {
            RolesListResponse response = new RolesListResponse();
            try
            {
                try
                {
                    response = (new RolesDao()).Read(1);
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
        /// Obtiene un registro por Id de la tabla Roles
        /// </summary>
        /// <remarks>
        /// Este EndPoint obtiene un registro
        /// #### Ejemplo de entrada
        /// ##### Regresa un registros de la tabla Roles
        /// <param name="id">Id del registro a consultar</param>
        /// <returns>Es un objeto de Roles estructura similar a la tabla Roles</returns>
        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName )]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(RolesRegistryResponse))]
        public RolesRegistryResponse GetListIdAsync([FromUri(Name = "id")] int id)
        {
            RolesRegistryResponse response = new RolesRegistryResponse();
            try
            {
                try
                {
                    response = (new RolesDao()).ReadId(id);
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
        /// Elimina un registro por Id de la tabla Roles
        /// </summary>
        /// <remarks>
        /// Este EndPoint obtiene un registro
        /// #### Ejemplo de entrada
        /// ##### Regresa un registros de la tabla Roles
        /// <param name="id">Id del registro a eliminar</param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(RolesDeleteResponse))]
        public RolesDeleteResponse DeleteIdRole([FromUri(Name = "id")] int id)
        {
            RolesDeleteResponse response = new RolesDeleteResponse();
            try
            {
                try
                {
                    response = (new RolesDao()).DeleteId(id);
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