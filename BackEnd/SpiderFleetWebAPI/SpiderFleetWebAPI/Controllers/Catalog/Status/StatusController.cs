using SpiderFleetWebAPI.Models.Request.Catalog.Status;
using SpiderFleetWebAPI.Models.Response.Catalog.Status;
using SpiderFleetWebAPI.Utils.Catalog.Status;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Net;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Catalog.Status
{
    public class StatusController : ApiController
    {
        private const string Tag = "Mantenimiento de Status";
        private const string BasicRoute = "api/";
        private const string ResourceName = "administration/status";
        

        /// <summary>
        /// Alta de Status
        /// </summary>
        /// <remarks>
        /// Este EndPoint nos genera el registro de la Status
        /// #### Ejemplo de entrada
        /// ##### Inserta un registro En la Tabla Status
        /// ```
        /// {
        /// "description" : "active"
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
        /// <param name="statusRequest">Objeto de entrada para el Endpoint</param>
        /// <returns>Es un objeto de Status estructura similar a la tabla Status</returns>
        [Authorize]
        [HttpPost]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(StatusResponse))]
        public StatusResponse Create([FromBody] StatusRequest statusRequest)
        {
            StatusResponse response = new StatusResponse();

            try
            {
                if (!(statusRequest is StatusRequest))
                {
                    response.success = false;
                    response.messages.Add("Objeto de entrada invalido");
                    return response;
                }

                CredencialSpiderFleet.Models.Catalogs.Status.Status status = new CredencialSpiderFleet.Models.Catalogs.Status.Status();
                status.Description = statusRequest.Description;

                try
                {
                    response = (new StatusDao()).Create(status);
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
        /// Actuliazacion de Status
        /// </summary>
        /// <remarks>
        /// Este EndPoint que actualiza el registro de la tabla status
        /// #### Ejemplo de entrada
        /// ##### Actualiza un resgistro en la Tabla Status
        /// ```
        /// {
        /// "idstatus":1,
        /// "description":"activo"
        /// }
        /// ```
        /// </remarks>
        /// <example>
        /// ```
        /// {
        /// "idstatus" : "int",
        /// "description":"string"
        /// }
        /// ```
        /// </example>
        /// <param name="statusRequest">Objeto de entrada para el Endpoint</param>
        /// <returns>Es un objeto de Status estructura similar a la tabla Status</returns>
        [Authorize]
        [HttpPut]
        [Route(BasicRoute + ResourceName )]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(StatusResponse))]
        public StatusResponse Update([FromBody] StatusUpdateRequest statusRequest)
        {
            StatusResponse response = new StatusResponse();
            try
            {
                if (!(statusRequest is StatusRequest))
                {
                    response.success = false;
                    response.messages.Add("Objeto de entrada invalido");
                    return response;
                }

                CredencialSpiderFleet.Models.Catalogs.Status.Status status = new CredencialSpiderFleet.Models.Catalogs.Status.Status();
                status.IdStatus = statusRequest.IdStatus;
                status.Description = statusRequest.Description;

                try
                {
                    response = (new StatusDao()).Update(status);
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
        /// Obtiene todos los registros de Status
        /// </summary>
        /// <remarks>
        /// Este EndPoint obtiene todos los registros de la Tabla Status
        /// <returns>Es una lista de Status estructura similar a la tabla Status</returns>
        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName + "/list")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(StatusResponse))]
        public StatusListResponse GetListAsync()
        {
            StatusListResponse response = new StatusListResponse();
            try
            {
                try
                {
                    response = (new StatusDao()).Read();
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
        /// Obtiene un registro por Id de la tabla Status
        /// </summary>
        /// <remarks>
        /// Este EndPoint obtiene un registro
        /// <param name="id">Id del registro a consultar</param>
        /// <returns>Es un objeto de Status estructura similar a la tabla Status</returns>
        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(StatusRegistryResponse))]
        public StatusRegistryResponse GetListIdAsync([FromUri(Name = "id")] int id)
        {
            StatusRegistryResponse response = new StatusRegistryResponse();
            try
            {
                try
                {
                    response = (new StatusDao()).ReadId(id);
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

