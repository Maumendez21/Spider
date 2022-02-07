using CredencialSpiderFleet.Models.Main.Reports;
using OfficeOpenXml;
using SpiderFleetWebAPI.Models.Response.ReportAdmin;
using SpiderFleetWebAPI.Models.Response.Sims;
using SpiderFleetWebAPI.Utils.Main.Reports;
using SpiderFleetWebAPI.Utils.Sims;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.ReportAdmin
{
    public class ReportByCompanyController : ApiController
    {

        //private const string Tag = "Reporte por Empresa de ";
        //private const string BasicRoute = "api/";
        //private const string ResourceName = "business/report";


        //[Authorize]
        [HttpGet]
        [Route("api/business/report/{id}")]
        //[Authorize]
        //[HttpPut]
        //[Route(BasicRoute + ResourceName)]
        //[SwaggerOperation(Tags = new[] { Tag })]
        //[SwaggerResponse(HttpStatusCode.OK, Type = typeof(BusinessReportResponse))]
        public async System.Threading.Tasks.Task<HttpResponseMessage> DescargaReporteSumarioAdquConcurso(string id)
        //public BusinessReportResponse BusinessReport([FromUri(Name = "id")] string id)
        {
            BusinessReportResponse response = new BusinessReportResponse();
            MemoryStream memoryStream = new MemoryStream();
            WebClient client = new WebClient();

            try
            {
                //memoryStream = new MemoryStream(client.DownloadData("C:/Simopa/Templete/TemplateBusinessReport.xlsx"));
                memoryStream = new MemoryStream(client.DownloadData("https://spiderfleetapi.azurewebsites.net/templates/reportes/TemplateBusinessReport.xlsx"));
            }
            finally
            {
                client.Dispose();
            }

            //ReportCreditSimsResponse list = new ReportCreditSimsResponse();

            //list = (new SimsMaintenanceDao()).ReporteCreditoSims();

            MemoryStream ms = new MemoryStream();
            //Envia datos a Excel
            using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
            {
                ExcelWorkbook excelWorkBook = excelPackage.Workbook;
                ExcelWorksheet excelWorksheet = excelWorkBook.Worksheets[1];
                excelWorksheet.Workbook.CalcMode = ExcelCalcMode.Manual;

                //Imagen
                //Image img = Image.FromStream(memoryImage);
                //ExcelPicture pic = excelWorksheet.Drawings.AddPicture("imageVista-" + 1, img);
                //pic.SetPosition(rowIndex, 0, colIndex + ((1 - 1) * 4), 0);
                //pic.SetSize(Width, Height);

                int rows = 8;
                int columns = 2;
                DateTime fecha = DateTime.Now;

                excelWorksheet.Cells[3, 10].Value = "Fecha:";
                excelWorksheet.Cells[3, 11].Value = DateTime.Now.ToString("dd/MM/yyyy");

                excelWorksheet.Cells[7, 2].Value = "Estatus";
                excelWorksheet.Cells[7, 3].Value = "Grupo";
                excelWorksheet.Cells[7, 4].Value = "Device";
                excelWorksheet.Cells[7, 5].Value = "Nombre";
                excelWorksheet.Cells[7, 6].Value = "Fecha";
                excelWorksheet.Cells[7, 7].Value = "Latitud";
                excelWorksheet.Cells[7, 8].Value = "Longitud";
                excelWorksheet.Cells[7, 9].Value = "Evento";                
                excelWorksheet.Cells[7, 10].Value = "Mode";
                excelWorksheet.Cells[7, 11].Value = "Alarma";
                excelWorksheet.Cells[7, 12].Value = "Saldo";
                excelWorksheet.Cells[7, 13].Value = "Version SW";
                excelWorksheet.Cells[7, 14].Value = "Version HW";
                excelWorksheet.Cells[7, 15].Value = "SIM";

                List<ReportByCompany> listData = new List<ReportByCompany>();

                listData = (new ReportByCompanyDao()).ReadReportLastEstatus(id);


                foreach (var data in listData)
                {
                    excelWorksheet.Cells[rows, columns].Value = data.StatusEvents; columns++;
                    excelWorksheet.Cells[rows, columns].Value = data.Empresa; columns++;
                    excelWorksheet.Cells[rows, columns].Value = data.Device; columns++;
                    excelWorksheet.Cells[rows, columns].Value = data.Name; columns++;
                    excelWorksheet.Cells[rows, columns].Value = data.Date.ToString("dd/MM/yyyy HH:mm:ss"); columns++;
                    excelWorksheet.Cells[rows, columns].Value = data.Latitude; columns++;
                    excelWorksheet.Cells[rows, columns].Value = data.Longitude; columns++;
                    excelWorksheet.Cells[rows, columns].Value = data.Event; columns++;                    
                    excelWorksheet.Cells[rows, columns].Value = data.Mode; columns++;
                    excelWorksheet.Cells[rows, columns].Value = data.Alarma; columns++;
                    excelWorksheet.Cells[rows, columns].Value = data.Saldo; columns++;
                    excelWorksheet.Cells[rows, columns].Value = data.VersionSW; columns++;
                    excelWorksheet.Cells[rows, columns].Value = data.VersionHW; columns++;
                    excelWorksheet.Cells[rows, columns].Value = data.Sim; columns++;


                    rows++;
                    columns = 2;
                }

                //excelWorksheet.Calculate();
                //excelWorksheet.Workbook.Calculate();

                excelPackage.SaveAs(ms);
            }

            ms.Position = 0;

            Spire.Xls.Workbook workbook = new Spire.Xls.Workbook();
            workbook.LoadFromStream(ms);

            var reporte = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ms.ToArray())
            };
            reporte.Content.Headers.ContentDisposition =
                new ContentDispositionHeaderValue("attachment")
                {
                    FileName = "BusinessReport.xlsx"
                };
            reporte.Content.Headers.ContentType =
                new MediaTypeHeaderValue("application/octet-stream");

            response.reporte = reporte;

            return reporte;
        }
    }
}
