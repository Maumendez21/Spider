using SpiderFleetWebAPI.Models.Request.Itinearios;
using SpiderFleetWebAPI.Models.Response.Itinerarios;
using SpiderFleetWebAPI.Utils.Itinerarios;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Itinerarios
{
    /// <summary>
    /// Controlador para proveer APIs relacionadas con los itinerarios historicos de un vehiculo
    /// </summary>
    public class ItinerariosController : ApiController
    {
        private const string Tag = "Itinerarios de Vehiculo";
        private const string BasicRoute = "api/v1/";
        private const string ResourceName = "itinerarios";

        /// <summary>
        /// Listado de itinerarios
        /// </summary>
        /// <remarks>
        /// Este Endpoint nos entrega un listado de todos itinerarios de un vehiculo en un periodo determinado
        /// #### Ejemplo de entrada
        /// ##### Busca todos los itinerarios de un vehiculo ID XXXX entre el 5 y 6 de diciembre
        /// ```
        /// {
        /// "idVehiculo" : "XXX",
        /// "dateInit" : "2019-12-05 00:00",
        /// "dateEnd" : "2019-12-06 23:59"
        /// }
        /// ```
        /// </remarks>
        /// <example>
        /// ```
        /// {
        /// "idEmpresa" : "string",
        /// "dateInit" : "datetime",
        /// "dateEnd" : "datetime"
        /// }
        /// ```
        /// </example>
        /// <param name="itinerariosListadoRequest">Objeto de entrada para el Endpoint</param>
        /// <returns>Un arreglo con el listado de los itinerarios de un vehiculo</returns>
        [HttpPost]
        [Route(BasicRoute + ResourceName + "/listado")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ItinerariosListResponse))]
        public ItinerariosListResponse ItinerariosListado(
            [FromBody][Required] ItinerariosListRequest itinerariosListadoRequest)
        {
            ItinerariosListResponse itinerariosListadoResponse = new ItinerariosListResponse();

            if (!(itinerariosListadoRequest is ItinerariosListRequest))
            {
                itinerariosListadoResponse.success = false;
                itinerariosListadoResponse.messages.Add("Objeto de entrada invalido");
                return itinerariosListadoResponse;
            }

            ItinerariosUtils itinerariosUtils = new ItinerariosUtils();

            itinerariosListadoResponse = itinerariosUtils.GetItinerariosList(itinerariosListadoRequest);

            return itinerariosListadoResponse;
        }


        [HttpPost]
        [Route(BasicRoute + ResourceName + "/ruta")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ItinerariosListResponse))]
        public ItinerariosListResponse ItinerariosRuta(
            [FromBody][Required] ItinerariosListRequest itinerariosListRequest)
        {
            ItinerariosListResponse itinerariosListResponse = new ItinerariosListResponse();

            if (!(itinerariosListRequest is ItinerariosListRequest))
            {
                itinerariosListResponse.success = false;
                itinerariosListResponse.messages.Add("Objeto de entrada invalido");
                return itinerariosListResponse;
            }

            ItinerariosUtils itinerariosUtils = new ItinerariosUtils();

            itinerariosListResponse = itinerariosUtils.GetItinerariosList(itinerariosListRequest);

            return itinerariosListResponse;
        }
    }
}