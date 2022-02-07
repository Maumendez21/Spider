using CredencialSpiderFleet.Models.Main.Reports;
using CredencialSpiderFleet.Models.Main.TraceTrip;
using SpiderFleetWebAPI.Models.Request.Main.Reports;
using SpiderFleetWebAPI.Models.Response.Main.Reports;
using SpiderFleetWebAPI.Utils.Main.Reports;
using SpiderFleetWebAPI.Utils.VerifyUser;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Main.Reports
{
    public class ReportsController : ApiController
    {

        private const string Tag = "Generacion de Reportes";
        private const string BasicRoute = "api/";
        private const string ResourceName = "spider/fleet/reports";

        [Authorize]
        [HttpPost]
        [Route(BasicRoute + ResourceName + "/driving/behaviors")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ReportsResponse))]
        public ReportsResponse GetReportDrivingBehaviors(ReportsRequest reportsRequest)
        {
            ReportsResponse response = new ReportsResponse();

            if (!(reportsRequest is ReportsRequest))
            {
                response.success = false;
                response.messages.Add("Objeto de entrada invalido");
                return response;
            }

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

            ReportsFiltros filtros = new ReportsFiltros();
            filtros.UserName = username;
            filtros.fechaInicio = reportsRequest.FechaInicial;
            filtros.fechaFin = reportsRequest.FechaFin;

            response.listPoints = (new ReportsDao()).Conducta(filtros);
            response.success = true;

            int respuesta = (new ReportsExcel()).excelDrivingBehaviors(response.listPoints);

            return response;
        }


        [Authorize]
        [HttpPost]
        [Route(BasicRoute + ResourceName + "/trips")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ReportsResponse))]
        public ReportsResponse GetReportTrips(ReportsRequest reportsRequest)
        {
            ReportsResponse response = new ReportsResponse();

            if (!(reportsRequest is ReportsRequest))
            {
                response.success = false;
                response.messages.Add("Objeto de entrada invalido");
                return response;
            }

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

            ReportsFiltros filtros = new ReportsFiltros();
            filtros.UserName = username;
            filtros.fechaInicio = reportsRequest.FechaInicial;
            filtros.fechaFin = reportsRequest.FechaFin;

            (new ReportsDao()).Trips(filtros);
            response.success = true;

            //int respuesta = (new ReportsExcel()).excelDrivingBehaviors(response.listPoints);

            return response;
        }
    }
}
