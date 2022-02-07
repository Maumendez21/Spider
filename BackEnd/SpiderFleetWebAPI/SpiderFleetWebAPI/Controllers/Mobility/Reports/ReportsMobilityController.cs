using SpiderFleetWebAPI.Models.Response.Responsible;
using SpiderFleetWebAPI.Utils.Mobility.Reports;
using SpiderFleetWebAPI.Utils.Responsible;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Mobility.Reports
{
    public class ReportsMobilityController : ApiController
    {

        [HttpGet]
        [AllowAnonymous]
        [Route("api/report/itinerarios/responsible")]
        public async System.Threading.Tasks.Task<HttpResponseMessage> ReportesItinerariosZip([FromUri] string grupo, [FromUri] string device,
            [FromUri] DateTime start, [FromUri] DateTime end)
        {

            try
            {
                MemoryStream memoryZip = new MemoryStream();

                ResponsibleVehicleResponse responsible = new ResponsibleVehicleResponse();
                responsible = (new ResponsibleDao()).ReadVehicle(device);
                String nombreVehiculo = string.Empty;

                if (!string.IsNullOrEmpty(responsible.responsible.Vehicle))
                {
                    nombreVehiculo = responsible.responsible.Vehicle;
                }
                else
                {
                    responsible = (new ResponsibleDao()).ReadNameVehicle(device);
                    nombreVehiculo = responsible.responsible.Vehicle;
                }

                string fileName = nombreVehiculo + ".zip";

                memoryZip = await (new ResponsibleItinerariesReportDao()).GeneraExcel(start, end, device, grupo, fileName, nombreVehiculo);

                var reporte = new HttpResponseMessage(HttpStatusCode.OK);
                reporte.Content = new ByteArrayContent(memoryZip.ToArray());
                reporte.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                reporte.Content.Headers.ContentDisposition.FileName = fileName; //"Archivos.zip";
                reporte.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                return reporte;
            }
            catch(Exception ex)
            {

                var reporte = new HttpResponseMessage(HttpStatusCode.NoContent);
                return reporte;

            }
        }



        [HttpGet]
        [AllowAnonymous]
        [Route("api/report/point/interest/analysis")]
        public HttpResponseMessage ReportesPointInterestZip([FromUri] string mongo, [FromUri] string grupo, [FromUri] string device,
        [FromUri] DateTime start, [FromUri] DateTime end)
        {

            try
            {
                MemoryStream memoryZip = new MemoryStream();

                ResponsibleVehicleResponse responsible = new ResponsibleVehicleResponse();
                responsible = (new ResponsibleDao()).ReadVehicle(device);
                String nombreVehiculo = string.Empty;

                if (!string.IsNullOrEmpty(responsible.responsible.Vehicle))
                {
                    nombreVehiculo = responsible.responsible.Vehicle;
                }
                else
                {
                    responsible = (new ResponsibleDao()).ReadNameVehicle(device);
                    nombreVehiculo = responsible.responsible.Vehicle;
                }

                string fileName = nombreVehiculo + ".zip";

                memoryZip = (new PointInterestAnalysisReportDao()).GeneraExcel(start, end, mongo, device, grupo, fileName, nombreVehiculo);

                var reporte = new HttpResponseMessage(HttpStatusCode.OK);
                reporte.Content = new ByteArrayContent(memoryZip.ToArray());
                reporte.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                reporte.Content.Headers.ContentDisposition.FileName = fileName; //"Archivos.zip";
                reporte.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                return reporte;
            }
            catch (Exception ex)
            {

                var reporte = new HttpResponseMessage(HttpStatusCode.NoContent);
                return reporte;

            }
        }


    }
}
