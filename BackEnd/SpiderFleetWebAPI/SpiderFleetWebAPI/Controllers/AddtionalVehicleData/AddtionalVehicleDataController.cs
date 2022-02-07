using SpiderFleetWebAPI.Models.Request.AddtionalVehicleData;
using SpiderFleetWebAPI.Models.Response.AddtionalVehicleData;
using SpiderFleetWebAPI.Utils.AddtionalVehicleData;
using SpiderFleetWebAPI.Utils.User;
using SpiderFleetWebAPI.Utils.VerifyUser;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Net;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.AddtionalVehicleData
{
    public class AddtionalVehicleDataController : ApiController
    {

        private const string Tag = "Datos Adicionales del Vehiculo";
        private const string BasicRoute = "api/";
        private const string ResourceName = "administration/addtional/data";

        /// <summary>
        /// Alta de Datos Adicionales del Vehiculo
        /// </summary>
        /// <remarks>
        /// Alta del registro 
        /// #### Ejemplo de entrada
        /// ```
        /// {
        /// "Device": "213WP2017005820",
        /// "IdMarca": "ACUR",
        /// "IdModelo": "ACMO",
        /// "IdVersion": "ACVE",
        /// "VIN": "EIRORIDMKD",
        /// "Placas": "IEWRN",
        /// "Poliza": "FUIGSHJBDF87",
        /// "IdTipoVehiculo": 2
        /// }
        /// ```
        /// </remarks>
        /// <example>
        /// ```
        /// {
        ///  "Device": "string",
        ///  "IdMarca": "string",
        ///  "IdModelo": "string",
        ///  "IdVersion": "string",
        ///  "VIN": "string",
        ///  "Placas": "string",
        ///  "Poliza": "string",
        ///  "IdTipoVehiculo": 0
        /// }
        /// ```
        /// </example>
        /// <param name="request">Objeto de entrada para el Endpoint</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route(BasicRoute + ResourceName )]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(AddtionalVehicleDataResponse))]
        public AddtionalVehicleDataResponse CreateUpdateAddtionalData(AddtionalVehicleDataRequest request)
        {
            AddtionalVehicleDataResponse response = new AddtionalVehicleDataResponse();
            try
            {

                //if (!(request is AddtionalVehicleDataResponse))
                //{
                //    response.success = false;
                //    response.messages.Add("Objeto de entrada invalido");
                //    return response;
                //}

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
                    CredencialSpiderFleet.Models.AddtionalVehicleData.AddtionalVehicleData addtional = new CredencialSpiderFleet.Models.AddtionalVehicleData.AddtionalVehicleData();
                    addtional.Device = request.Device;
                    addtional.IdMarca = request.IdMarca;
                    addtional.IdModelo = request.IdModelo;
                    addtional.IdVersion = request.IdVersion;
                    addtional.VIN = request.VIN;
                    addtional.Placas = request.Placas;
                    addtional.Poliza = request.Poliza;
                    addtional.IdTipoVehiculo = request.IdTipoVehiculo;

                    response = (new AddtionalVehicleDataDao()).Create(addtional);

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
        /// Obtiene todos los registros de la Empresa
        /// </summary>
        /// <remarks>
        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName + "/list")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(AddtionalVehicleDataListResponse))]
        public AddtionalVehicleDataListResponse GetListAsync([FromUri(Name = "search")] string search)
        {
            AddtionalVehicleDataListResponse response = new AddtionalVehicleDataListResponse();

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
                try
                {
                    response = (new AddtionalVehicleDataDao()).Read(hierarchy, search);
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
        /// Obtiene un registro por numero de dispositivo
        /// </summary>
        /// <remarks>
        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(AddtionalVehicleDataRegistryResponse))]
        public AddtionalVehicleDataRegistryResponse GetListIdAsync([FromUri(Name = "device")] string device)
        {
            AddtionalVehicleDataRegistryResponse response = new AddtionalVehicleDataRegistryResponse();

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
                    response = (new AddtionalVehicleDataDao()).ReadId(device);
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
