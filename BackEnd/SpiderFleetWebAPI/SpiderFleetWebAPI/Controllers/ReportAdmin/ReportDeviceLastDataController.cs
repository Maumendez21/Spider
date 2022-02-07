using CredencialSpiderFleet.Models.ReportAdmin;
using OfficeOpenXml;
using SpiderFleetWebAPI.Models.Response.ReportAdmin;
using SpiderFleetWebAPI.Utils.ReportAdmin;
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
    public class ReportDeviceLastDataController : ApiController
    {
        [HttpGet]
        [Route("api/report/device/data/last/{device}")]
        public async System.Threading.Tasks.Task<HttpResponseMessage> DescargaReporteLino(string device)
        {
            ReportDeviceLastDataResponse response = new ReportDeviceLastDataResponse();
            BusinessReportResponse reportes = new BusinessReportResponse();
            MemoryStream memoryStream = new MemoryStream();
            WebClient client = new WebClient();

            try
            {
                memoryStream = new MemoryStream(client.DownloadData("C:/Simopa/Templete/TempleteReporte.xlsx"));
                //memoryStream = new MemoryStream(client.DownloadData("https://spiderfleetapi.azurewebsites.net/templates/reportes/TempleteReporte.xlsx"));
            }
            finally
            {
                client.Dispose();
            }

            MemoryStream ms = new MemoryStream();
            //Envia datos a Excel
            using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
            {
                ExcelWorkbook excelWorkBook = excelPackage.Workbook;
                ExcelWorksheet excelWorksheet = excelWorkBook.Worksheets[1];
                excelWorksheet.Workbook.CalcMode = ExcelCalcMode.Manual;

                int rows = 5;
                int columns = 2;
                DateTime fecha = DateTime.Now;

                excelWorksheet.Cells[3, 10].Value = "Fecha:";
                excelWorksheet.Cells[3, 11].Value = DateTime.Now.ToString("dd/MM/yyyy");

                excelWorksheet.Cells[rows, columns].Value = "Evento"; columns++;
                excelWorksheet.Cells[rows, columns].Value = "Device"; columns++;
                excelWorksheet.Cells[rows, columns].Value = "Alarma"; columns++;
                excelWorksheet.Cells[rows, columns].Value = "Fecha"; columns++;
                excelWorksheet.Cells[rows, columns].Value = "Mode"; columns++;
                excelWorksheet.Cells[rows, columns].Value = "Longitud"; columns++;
                excelWorksheet.Cells[rows, columns].Value = "Latitud"; columns++;
                excelWorksheet.Cells[rows, columns].Value = "VersionSW"; columns++;
                excelWorksheet.Cells[rows, columns].Value = "VerisionHW"; columns++;

                rows++;
                columns = 2;

                List<ReportDeviceLastData> listAlarm = new List<ReportDeviceLastData>();
                List<ReportDeviceLastData> listLogin = new List<ReportDeviceLastData>();

                response = (new ReportDeviceLastDataDao()).GetDataMongo(device);

                listAlarm = response.listAlarm;
                listLogin = response.listLogin;


                foreach (var data in listLogin)
                {
                    excelWorksheet.Cells[rows, columns].Value = data.Event; columns++;
                    excelWorksheet.Cells[rows, columns].Value = data.Device; columns++;
                    excelWorksheet.Cells[rows, columns].Value = data.Alarma; columns++;
                    excelWorksheet.Cells[rows, columns].Value = data.Date.ToString("dd/MM/yyyy HH:mm:ss"); columns++;
                    excelWorksheet.Cells[rows, columns].Value = data.Mode; columns++;
                    excelWorksheet.Cells[rows, columns].Value = data.Longitude; columns++;
                    excelWorksheet.Cells[rows, columns].Value = data.Latitude; columns++;
                    excelWorksheet.Cells[rows, columns].Value = data.VersionSW; columns++;
                    excelWorksheet.Cells[rows, columns].Value = data.VersionHW; columns++;

                    rows++;
                    columns = 2;
                }


                excelWorksheet.Cells[rows, columns].Value = "Evento"; columns++;
                excelWorksheet.Cells[rows, columns].Value = "Device"; columns++;
                excelWorksheet.Cells[rows, columns].Value = "Alarma"; columns++;
                excelWorksheet.Cells[rows, columns].Value = "Fecha"; columns++;
                excelWorksheet.Cells[rows, columns].Value = "Mode"; columns++;
                excelWorksheet.Cells[rows, columns].Value = "Longitud"; columns++;
                excelWorksheet.Cells[rows, columns].Value = "Latitud"; columns++;
                excelWorksheet.Cells[rows, columns].Value = "VersionSW"; columns++;
                excelWorksheet.Cells[rows, columns].Value = "VerisionHW"; columns++;

                rows++;
                columns = 2;

                foreach (var data in listAlarm)
                {
                    excelWorksheet.Cells[rows, columns].Value = data.Event; columns++;
                    excelWorksheet.Cells[rows, columns].Value = data.Device; columns++;
                    excelWorksheet.Cells[rows, columns].Value = data.Alarma; columns++;
                    excelWorksheet.Cells[rows, columns].Value = data.Date.ToString("dd/MM/yyyy HH:mm:ss"); columns++;
                    excelWorksheet.Cells[rows, columns].Value = data.Mode; columns++;
                    excelWorksheet.Cells[rows, columns].Value = data.Longitude; columns++;
                    excelWorksheet.Cells[rows, columns].Value = data.Latitude; columns++;
                    excelWorksheet.Cells[rows, columns].Value = data.VersionSW; columns++;
                    excelWorksheet.Cells[rows, columns].Value = data.VersionHW; columns++;

                    rows++;
                    columns = 2;
                }

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

            reportes.reporte = reporte;

            return reporte;
        }
    }
}
