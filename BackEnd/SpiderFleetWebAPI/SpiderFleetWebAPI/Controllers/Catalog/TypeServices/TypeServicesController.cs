using SpiderFleetWebAPI.Models.Request.Catalog.TypeService;
using SpiderFleetWebAPI.Models.Response.Catalog.TypeService;
using SpiderFleetWebAPI.Utils.Catalog.TypeService;
using SpiderFleetWebAPI.Utils.VerifyUser;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Catalog.TypeServices
{
    public class TypeServicesController : ApiController
    {
        private const string Tag = "Mantenimiento de Tipos de Servicios";
        private const string BasicRoute = "api/";
        private const string ResourceName = "administration/type/Services";
        private CredencialSpiderFleet.Models.Useful.UseFul useful = new CredencialSpiderFleet.Models.Useful.UseFul();

        /// <summary>
        /// Alta de Tipo de Servicio
        /// </summary>
        /// <remarks>
        /// Este EndPoint nos genera un registro en TypeServices 
        /// #### Ejemplo de entrada
        /// ##### Inserta un registro En la Tabla TypeServices 
        /// ```
        /// {
        /// "description" : "Cambio de Aceite"
        /// "estimatedTime" : "2:00 hrs"
        /// "estimateCost" : 300
        /// }
        /// ```
        /// </remarks>
        /// <example>
        /// ```
        /// {
        /// "description" : "string"
        /// "estimatedTime" : "string"
        /// "estimateCost" : "double"
        /// }
        /// ```
        /// </example>
        /// <param name="typeservicerequest">Objeto de entrada para el Endpoint</param>
        /// <returns>Es un objeto de TypeService estructura similar a la tabla Tipo Servicio</returns>
        [Authorize]
        [HttpPost]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(TypeServiceResponse))]
        public TypeServiceResponse Create([FromBody] TypeServiceRequest typeservicerequest)
        {
            TypeServiceResponse response = new TypeServiceResponse();

            try
            {
                if (!(typeservicerequest is TypeServiceRequest))
                {
                    response.success = false;
                    response.messages.Add("Objeto de entrada invalido");
                    return response;
                }

                CredencialSpiderFleet.Models.Catalogs.TypeService.TypeService typeService = new CredencialSpiderFleet.Models.Catalogs.TypeService.TypeService();
                typeService.Description = typeservicerequest.Description;
                typeService.EstimatedTime = typeservicerequest.EstimatedTime;
                typeService.EstimatedCost = typeservicerequest.EstimatedCost;
                try
                {
                    response = (new TypeServiceDao()).Create(typeService);

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
        /// Actualiazacion de Tipo de Servicio
        /// </summary>
        /// <remarks>
        /// Este EndPoint que actualiza el registro de la tabla TypeServices
        /// #### Ejemplo de entrada
        /// ##### Actualiza un resgistro en la Tabla TypeServices
        /// ```
        /// {
        /// "id":1,
        /// "description" : "Cambio de Aceite"
        /// "estimatedTime" : "2:00 hrs"
        /// "estimateCost" : "300"
        /// }
        /// ```
        /// </remarks>
        /// <example>
        /// ```
        /// {
        /// "id" : "int",
        /// "description" : "string"
        /// "estimatedTime" : "string"
        /// "estimateCost" : "double"
        /// }
        /// ```
        /// </example>
        /// <param name="typeservicerequest">Objeto de entrada para el Endpoint</param>
        /// <returns>Es un objeto de TypeService estructura similar a la tabla Tipo Servicio</returns>
        [Authorize]
        [HttpPut]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(TypeServiceResponse))]
        public TypeServiceResponse Update([FromBody] TypeServiceUpdateRequest typeservicerequest)
        {
            TypeServiceResponse response = new TypeServiceResponse();
            try
            {
                if (!(typeservicerequest is TypeServiceUpdateRequest))
                {
                    response.success = false;
                    response.messages.Add("Objeto de entrada invalido");
                    return response;
                }

                CredencialSpiderFleet.Models.Catalogs.TypeService.TypeService typeService = new CredencialSpiderFleet.Models.Catalogs.TypeService.TypeService();
                typeService.IdTypeService = typeservicerequest.IdTypeService;
                typeService.Description = typeservicerequest.Description;
                typeService.EstimatedTime = typeservicerequest.EstimatedTime;
                typeService.EstimatedCost = typeservicerequest.EstimatedCost;

                try
                {
                    response = (new TypeServiceDao()).Update(typeService);
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
        /// Lista de Tipos de Servicios
        /// </summary>
        /// <remarks>
        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName + "/list")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(TypeServiceListResponse))]
        public TypeServiceListResponse GetListAsync()
        {
            TypeServiceListResponse response = new TypeServiceListResponse();
            try
            {
                string username = string.Empty;
                //string hierarchy = string.Empty;

                try
                {
                    username = (new VerifyUser()).verifyTokenUser(User);
                    //hierarchy = (new UserDao()).ReadUserHierarchy(username);
                }
                catch (Exception ex)
                {
                    response.success = false;
                    response.messages.Add(ex.Message);
                    return response;
                }

                try
                {
                    response = (new TypeServiceDao()).Read();
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
        /// Obtiene un registro por Id de la tabla TypeServices
        /// </summary>
        /// <remarks>
        /// Este EndPoint obtiene un registro
        /// #### Ejemplo de entrada
        /// ##### Regresa un registros de la tabla TypeServices
        /// <param name="id">Id del registro a consultar</param>
        /// <returns>Es un objeto de TypeServices estructura similar a la tabla Tipo Servicio</returns>
        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(TypeServiceRegistryResponse))]
        public TypeServiceRegistryResponse GetListIdAsync([FromUri(Name = "id")] int id)
        {
            TypeServiceRegistryResponse response = new TypeServiceRegistryResponse();
            try
            {
                try
                {
                    response = (new TypeServiceDao()).ReadId(id);
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
        /// Elimina un registro por Id de la tabla TypeServices
        /// </summary>
        /// <remarks>
        /// Este EndPoint obtiene un registro
        /// #### Ejemplo de entrada
        /// ##### Regresa un registros de la tabla TypeServices
        /// <param name="id">Id del registro a eliminar</param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(TypeServiceDeleteResponse))]
        public TypeServiceDeleteResponse DeleteIdRole([FromUri(Name = "id")] int id)
        {
            TypeServiceDeleteResponse response = new TypeServiceDeleteResponse();
            try
            {
                try
                {
                    response = (new TypeServiceDao()).DeleteId(id);
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