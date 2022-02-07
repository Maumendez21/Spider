using SpiderFleetWebAPI.Models.Request.Status;
using SpiderFleetWebAPI.Models.Response.Status;
using SpiderFleetWebAPI.Utils.Status;
using Swashbuckle.Swagger.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Mapa
{
    public class MapaController : ApiController
    {
        private const string Tag = "Ubicación y ultimo status";
        private const string BasicRoute = "api/";
        private const string ResourceName = "status";

        /// <summary>
        /// Ubicación y último estado de flotilla
        /// </summary>
        /// <remarks>
        /// Este Endpoint nos entrega un listado de todos los vehiculos existentes en una flotilla de una determinada empresa
        /// #### Ejemplo de entrada
        /// ##### Busca todos los autos de la empresa con ID 1
        /// ```
        /// {
        /// "idEmpresa" : "1"
        /// }
        /// ```
        /// </remarks>
        /// <example>
        /// ```
        /// {
        /// "idEmpresa" : "string"
        /// }
        /// ```
        /// </example>
        /// <param name="statusFleetRequest">Objeto de entrada para el Endpoint</param>
        /// <returns>Un arreglo con la información de ubicación y status de una flotilla</returns>
        [HttpPost]
        [Route(BasicRoute + ResourceName + "/flotilla")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(StatusFleetResponse))]
        public StatusFleetResponse StatusFlotilla(
            [FromBody][Required] StatusFleetRequest statusFleetRequest)
        {
            StatusFleetResponse statusFleetResponse = new StatusFleetResponse();

            if (!(statusFleetRequest is StatusFleetRequest))
            {
                statusFleetResponse.success = false;
                statusFleetResponse.messages.Add("Objeto de entrada invalido");
                return statusFleetResponse;
            }

            StatusUtils mapaUtils = new StatusUtils();

            statusFleetResponse = mapaUtils.GetStatusFlotilla(statusFleetRequest);

            return statusFleetResponse;
        }

        /// <summary>
        /// Ubicación y último estado de vehiculo
        /// </summary>
        /// <remarks>
        /// Este Endpoint nos entrega la ubicación y el último estado de un vehiculo determinado
        /// #### Ejemplo de entrada
        /// ##### Busca los datos de un vehiculo con ID XXXX
        /// ```
        /// {
        /// "idVehiculo" : "XXXX"
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
        /// <param name="statusVehiculoRequest">Objeto de entrada para el Endpoint</param>
        /// <returns>Un arreglo con la información de ubicación y status de un vehiculo</returns>
        [HttpPost]
        [Route(BasicRoute + ResourceName + "/vehiculo")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(StatusVehiculoResponse))]
        public StatusVehiculoResponse StatusVehiculo(
            [FromBody][Required] StatusVehiculoRequest statusVehiculoRequest)
        {
            StatusVehiculoResponse statusVehiculoResponse = new StatusVehiculoResponse();

            if (!(statusVehiculoRequest is StatusVehiculoRequest))
            {
                statusVehiculoResponse.success = false;
                statusVehiculoResponse.messages.Add("Objeto de entrada invalido");
                return statusVehiculoResponse;
            }

            StatusUtils mapaUtils = new StatusUtils();

            statusVehiculoResponse = mapaUtils.GetStatusVehiculo(statusVehiculoRequest);

            return statusVehiculoResponse;
        }
    }
}