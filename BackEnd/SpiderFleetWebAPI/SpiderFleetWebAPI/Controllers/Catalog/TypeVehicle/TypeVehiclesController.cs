using SpiderFleetWebAPI.Models.Request.Catalog.TypeVehicle;
using SpiderFleetWebAPI.Models.Response.Catalog.TypeVehicle;
using SpiderFleetWebAPI.Utils.Catalog.TypeVehicle;
using SpiderFleetWebAPI.Utils.VerifyUser;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Net;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Catalog.TypeVehicle
{
    public class TypeVehiclesController : ApiController
    {
        private const string Tag = "Mantenimiento de Tipos de Vehiculos";
        private const string BasicRoute = "api/";
        private const string ResourceName = "administration/type/vehicles";
        private CredencialSpiderFleet.Models.Useful.UseFul useful = new CredencialSpiderFleet.Models.Useful.UseFul();



        /// <summary>
        /// Alta de Tipo de Vehiculo
        /// </summary>
        /// <remarks>
        /// Este EndPoint nos genera un registro en VehicleType
        /// #### Ejemplo de entrada
        /// ##### Inserta un registro En la Tabla VehicleType
        /// ```
        /// {
        /// "description" : "Motocileta"
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
        /// <param name="typevehiclerequest">Objeto de entrada para el Endpoint</param>
        /// <returns>Es un objeto de VehicleType estructura similar a la tabla Tipo Vehiculo</returns>
        [Authorize]
        [HttpPost]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(TypeVehicleResponse))]
        public TypeVehicleResponse Create([FromBody] TypeVehicleRequest typevehiclerequest)
        {
            TypeVehicleResponse response = new TypeVehicleResponse();

            try
            {
                if (!(typevehiclerequest is TypeVehicleRequest))
                {
                    response.success = false;
                    response.messages.Add("Objeto de entrada invalido");
                    return response;
                }

                CredencialSpiderFleet.Models.Catalogs.TypeVehicle.TypeVehicle TypeVehicle = new CredencialSpiderFleet.Models.Catalogs.TypeVehicle.TypeVehicle();
                TypeVehicle.Description = typevehiclerequest.Description;
                try
                {
                    response = (new TypeVehicleDao()).Create(TypeVehicle);

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
        /// Actualiazacion de Tipo de Vehiculo
        /// </summary>
        /// <remarks>
        /// Este EndPoint que actualiza el registro de la tabla VehicleType
        /// #### Ejemplo de entrada
        /// ##### Actualiza un resgistro en la Tabla VehicleType
        /// ```
        /// {
        /// "id":1,
        /// "description":"Camion"
        /// }
        /// ```
        /// </remarks>
        /// <example>
        /// ```
        /// {
        /// "id" : "int",
        /// "description":"string"
        /// }
        /// ```
        /// </example>
        /// <param name="typevehiclerequest">Objeto de entrada para el Endpoint</param>
        /// <returns>Es un objeto de VehicleType estructura similar a la tabla Tipo Vehiculo</returns>
        [Authorize]
        [HttpPut]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(TypeVehicleResponse))]
        public TypeVehicleResponse Update([FromBody] TypeVehicleUpdateRequest typevehiclerequest)
        {
            TypeVehicleResponse response = new TypeVehicleResponse();
            try
            {
                if (!(typevehiclerequest is TypeVehicleUpdateRequest))
                {
                    response.success = false;
                    response.messages.Add("Objeto de entrada invalido");
                    return response;
                }

                CredencialSpiderFleet.Models.Catalogs.TypeVehicle.TypeVehicle TypeVehicle = new CredencialSpiderFleet.Models.Catalogs.TypeVehicle.TypeVehicle();
                TypeVehicle.IdTypeVehicle = typevehiclerequest.IdTypeVehicle;
                TypeVehicle.Description = typevehiclerequest.Description;

                try
                {
                    response = (new TypeVehicleDao()).Update(TypeVehicle);
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
        /// Lista de Tipos de Autos
        /// </summary>
        /// <remarks>
        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName + "/list")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(TypeVehicleListResponse))]
        public TypeVehicleListResponse GetListAsync()
        {
            TypeVehicleListResponse response = new TypeVehicleListResponse();
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
                    response = (new TypeVehicleDao()).Read();
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
        /// Obtiene un registro por Id de la tabla VehicleType
        /// </summary>
        /// <remarks>
        /// Este EndPoint obtiene un registro
        /// #### Ejemplo de entrada
        /// ##### Regresa un registros de la tabla VehicleType
        /// <param name="id">Id del registro a consultar</param>
        /// <returns>Es un objeto de VehicleType estructura similar a la tabla Tipo Vehiculo</returns>
        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(TypeVehicleRegistryResponse))]
        public TypeVehicleRegistryResponse GetListIdAsync([FromUri(Name = "id")] int id)
        {
            TypeVehicleRegistryResponse response = new TypeVehicleRegistryResponse();
            try
            {
                try
                {
                    response = (new TypeVehicleDao()).ReadId(id);
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
        /// Elimina un registro por Id de la tabla VehicleType
        /// </summary>
        /// <remarks>
        /// Este EndPoint obtiene un registro
        /// #### Ejemplo de entrada
        /// ##### Regresa un registros de la tabla VehicleType
        /// <param name="id">Id del registro a eliminar</param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(TypeVehicleDeleteResponse))]
        public TypeVehicleDeleteResponse DeleteIdRole([FromUri(Name = "id")] int id)
        {
            TypeVehicleDeleteResponse response = new TypeVehicleDeleteResponse();
            try
            {
                try
                {
                    response = (new TypeVehicleDao()).DeleteId(id);
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
