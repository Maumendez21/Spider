using SpiderFleetWebAPI.Models.Request.Inventory.CatalogStatusDevices;
using SpiderFleetWebAPI.Models.Response.Inventory.CatalogStatusDevices;
using SpiderFleetWebAPI.Utils.Inventory.CatalogStatusDevices;
using SpiderFleetWebAPI.Utils.VerifyUser;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Net;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Inventory.CatalogStatusDevices
{
    public class CatalogStatusDevicesController : ApiController
    {
        private const string Tag = "Mantenimiento de Catalogo Status de Dispositivos";
        private const string BasicRoute = "api/";
        private const string ResourceName = "inventario/catalogstatusdevices";

        /// <summary>
        /// Alta de Roles
        /// </summary>
        /// <remarks>
        /// Este EndPoint nos genera el registro de la comañia
        /// #### Ejemplo de entrada
        /// ##### Busca los datos de un vehiculo con ID XXXX
        /// ```
        /// {
        /// "idsuscriptiontype" : 3
        /// }
        /// ```
        /// </remarks>
        /// <example>
        /// ```
        /// {
        /// "idVehiculo" : "string"
        /// }
        /// ```
        /// </example>
        /// <param name="simRequest">Objeto de entrada para el Endpoint</param>
        /// <returns>Es un objeto de Company estructura similar a la tabla Companies</returns>
        [Authorize]
        [HttpPost]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(CatalogStatusDevicesResponse))]
        public CatalogStatusDevicesResponse Create([FromBody] CatalogStatusDevicesRequest simRequest)
        {
            CatalogStatusDevicesResponse response = new CatalogStatusDevicesResponse();
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
                if (!(simRequest is CatalogStatusDevicesRequest))
                {
                    response.success = false;
                    response.messages.Add("Objeto de entrada invalido");
                    return response;
                }
               
                CredencialSpiderFleet.Models.Inventory.CatalogStatusDevices.CatalogStatusDevices sim = new CredencialSpiderFleet.Models.Inventory.CatalogStatusDevices.CatalogStatusDevices();
                sim.Name = simRequest.Name;

                try
                {
                    response = (new CatalogStatusDevicesDao()).Create(sim);
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
        [Route(BasicRoute + ResourceName )]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(CatalogStatusDevicesResponse))]
        public CatalogStatusDevicesResponse Update([FromBody] CatalogStatusDevicesUpdateRequest simRequest)
        {
            CatalogStatusDevicesResponse response = new CatalogStatusDevicesResponse();
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
                if (!(simRequest is CatalogStatusDevicesRequest))
                {
                    response.success = false;
                    response.messages.Add("Objeto de entrada invalido");
                    return response;
                }

                CredencialSpiderFleet.Models.Inventory.CatalogStatusDevices.CatalogStatusDevices statusDevice = new CredencialSpiderFleet.Models.Inventory.CatalogStatusDevices.CatalogStatusDevices();
                statusDevice.IdStatus = simRequest.IdStatus;
                statusDevice.Name = simRequest.Name;

                try
                {
                    response = (new CatalogStatusDevicesDao()).Update(statusDevice);
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
        [Route(BasicRoute + ResourceName + "/list")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(CatalogStatusDevicesRegistryResponse))]
        public CatalogStatusDevicesListResponse GetListAsync()
        {
            CatalogStatusDevicesListResponse response = new CatalogStatusDevicesListResponse();
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
                    response = (new CatalogStatusDevicesDao()).Read();
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
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(CatalogStatusDevicesRegistryResponse))]
        public CatalogStatusDevicesRegistryResponse GetListIdAsync([FromUri(Name = "id")] int id)
        {
            CatalogStatusDevicesRegistryResponse response = new CatalogStatusDevicesRegistryResponse();
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
                    response = (new CatalogStatusDevicesDao()).ReadId(id);
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


