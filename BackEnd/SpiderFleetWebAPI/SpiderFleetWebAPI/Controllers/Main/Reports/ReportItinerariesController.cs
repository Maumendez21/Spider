using CredencialSpiderFleet.Models.Main.Filtros;
using CredencialSpiderFleet.Models.Main.Reports;
using SpiderFleetWebAPI.Models.Request.Main.Reports;
using SpiderFleetWebAPI.Models.Response.Main.Reports;
using SpiderFleetWebAPI.Utils.Main.Reports;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Main.Reports
{
    public class ReportItinerariesController : ApiController
    {

        private const string Tag = "Generacion de Reportes Itinerarios";
        private const string BasicRoute = "api/";
        private const string ResourceName = "spider/fleet/reports/itinerarios";

        //[Authorize]
        [HttpPost]
        [Route(BasicRoute + ResourceName )]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ReportItinerariesResponse))]
        public ReportItinerariesResponse GetReportDrivingBehaviors(ReportItinerariesRequest reportsRequest)
         {
            ReportItinerariesResponse response = new ReportItinerariesResponse();

            if (!(reportsRequest is ReportItinerariesRequest))
            {
                response.success = false;
                response.messages.Add("Objeto de entrada invalido");
                return response;
            }

            //string hierarchy = string.Empty;
            //var identity = (ClaimsIdentity)User.Identity;
            //var claim = identity.Claims.ToList();
            //var username = claim?.FirstOrDefault(x => x.Type.Equals("username", StringComparison.OrdinalIgnoreCase))?.Value;

            //if (string.IsNullOrEmpty(username))
            //{
            //    response.success = false;
            //    response.messages.Add("No contiene el UserName");
            //    return response;
            //}

            FilterItineraries filtros = new FilterItineraries();
            filtros.Device = reportsRequest.Dispositivo;
            filtros.StartDate = reportsRequest.FechaInicial;
            filtros.EndDate = reportsRequest.FechaFinal;

            response = (new ReporteItinerariosDao()).Read(filtros);

            //List<ReportHeaderTrip> header = new List<ReportHeaderTrip>();
            //List<ReportTrip> reportTrips = new List<ReportTrip>();

            //header = response.listHeader;
            //reportTrips = response.listTrip;

            return response;// Ok(header + " " + reportTrips);
        }

    }
}
