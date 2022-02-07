using CredencialSpiderFleet.Models.Main.HeatPoints;
using CredencialSpiderFleet.Models.Main.TraceTrip;
using SpiderFleetWebAPI.Models.Request.Main.LastTrip;
using SpiderFleetWebAPI.Models.Response.Main.HeatTrip;
using SpiderFleetWebAPI.Models.Response.Main.TraceTrip;
using SpiderFleetWebAPI.Utils.Main.LastTrip;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Main.LastTrip
{
    public class LastTripController : ApiController
    {

        private const string Tag = "Trazo de viajes ";
        private const string BasicRoute = "api/";
        private const string ResourceName = "trace";


        [HttpPost]
        [Route(BasicRoute + ResourceName + "/trip")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(TraceTripResponse))]
        public TraceTripResponse TraceTripMarkers(
           [FromBody][Required] TraceTripRequest lastTrip)
        {
            TraceTripResponse response = new TraceTripResponse();

            if (!(lastTrip is TraceTripRequest))
            {
                response.success = false;
                response.messages.Add("Objeto de entrada invalido");
                return response;
            }
            
            TraceTrip trace = new TraceTrip();
            trace.Device = lastTrip.Device;
            trace.fechaInicio =lastTrip.fechaInicio;
            trace.fechaFin = lastTrip.fechaFin;

            try
            {
                response = (new TraceTripDao()).GetInformation(trace);
            }
            catch(Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }

            return response;
        }


        [HttpPost]
        [Route(BasicRoute + ResourceName + "/heat")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(HeatTripResponse))]
        public HeatTripResponse HeatTraceTrip(
           [FromBody][Required] TraceTripRequest lastTrip)
        {
            HeatTripResponse response = new HeatTripResponse();

            if (!(lastTrip is TraceTripRequest))
            {
                response.success = false;
                response.messages.Add("Objeto de entrada invalido");
                return response;
            }
            List<Point> listPoints = new List<Point>();
            List<HeatPoints> listHeat = new List<HeatPoints>();

            try
            {
                listHeat = (new TraceTripDao()).GetHeatPoints(lastTrip);
                response.listHeats = listHeat;
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }

            return response;
        }
    }
}
