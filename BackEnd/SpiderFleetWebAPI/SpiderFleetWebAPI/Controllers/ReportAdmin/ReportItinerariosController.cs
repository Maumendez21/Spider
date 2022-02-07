using CredencialSpiderFleet.Models.Useful;
using ICSharpCode.SharpZipLib.Zip;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using SpiderFleetWebAPI.Models.Response.General;
using SpiderFleetWebAPI.Models.Response.ReportAdmin;
using SpiderFleetWebAPI.Models.Response.Responsible;
using SpiderFleetWebAPI.Utils.General;
using SpiderFleetWebAPI.Utils.Logo;
using SpiderFleetWebAPI.Utils.ReportAdmin;
using SpiderFleetWebAPI.Utils.Responsible;
using SpiderFleetWebAPI.Utils.Setting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Web.Hosting;
using System.Web.Http;


namespace SpiderFleetWebAPI.Controllers.ReportAdmin
{
    public class ReportItinerariosController : ApiController
    {
        private const string Tag = "Reportes Itinerarios por Dispositivo";
        private const string BasicRoute = "api/";
        private const string ResourceName = "report/";

        private UseFul use = new UseFul();
        private const string MXV = "MXV";
        private string path = HostingEnvironment.MapPath("/TempFiles/");

        [HttpGet]
        [AllowAnonymous]
        [Route("api/report/itinerarios/zip")]
        public async System.Threading.Tasks.Task<HttpResponseMessage> ReportesItinerariosZip([FromUri] int empresa, [FromUri] string param, [FromUri] int type,
            [FromUri] string grupo, [FromUri] string device,
            [FromUri] DateTime fechaInicio, [FromUri] DateTime fechaFin)
        {

            var fileName = string.Empty;
            var tempOutput = string.Empty;

            #region 

            DeviceResponse listDevice = new DeviceResponse();
            BusinessReportResponse response = new BusinessReportResponse();
            MemoryStream memoryStream = new MemoryStream();
            WebClient client = new WebClient();
            ReportItinerarioResponse information = new ReportItinerarioResponse();
            string nombreVehiculo = string.Empty;

            string parametro = use.hierarchyPrincipal(param);
            decimal maxVelocidad = Convert.ToDecimal((new SettingConfig()).ReadIdHerarchy(use.hierarchyPrincipal(param), MXV, 1));

            try
            {
                //memoryStream = new MemoryStream(client.DownloadData("C:/Simopa/Templete/TemplateItinerarios.xlsx"));

                if(type == 2)//.Equals("/79/"))
                {
                    memoryStream = new MemoryStream(client.DownloadData("https://spiderfleetapi.azurewebsites.net/templates/reportes/TemplateItinerariosPetro.xlsx"));
                }
                else if (type == 1)
                {
                    memoryStream = new MemoryStream(client.DownloadData("https://spiderfleetapi.azurewebsites.net/templates/reportes/TemplateItinerarios.xlsx"));
                }
            }
            finally
            {
                client.Dispose();
            }

            List<string> filesExcel = new List<string>();
            List<string> devices = new List<string>();

            if(!string.IsNullOrEmpty(grupo))
            {
                if (!string.IsNullOrEmpty(device))
                {
                    devices.Add(device);
                    listDevice = (new GeneralDao()).ReadGroupName(device);
                    fileName = listDevice.Grupo + ".zip";
                }
                else
                {
                    listDevice = (new GeneralDao()).ReadDeviceByGrupo(grupo);
                    devices = listDevice.ListDevice;
                    fileName = listDevice.Grupo + ".zip";
                }                    
            }
            else
            {
                devices.Add(device);
                listDevice = (new GeneralDao()).ReadGroupName(device);
                fileName = listDevice.Grupo + ".zip";
            }

            tempOutput = path + fileName;

            #region Logo de Empresa

            //Imagen           
            int Height = 0;
            int Width = 0;
            int rowIndex = 0;
            int colIndex = 0;
            string nameImage = string.Empty;

            WebClient clientImage = new WebClient();
            MemoryStream memoryImage = new MemoryStream();
            try
            {

                CredencialSpiderFleet.Models.Logo.Logo logo = new CredencialSpiderFleet.Models.Logo.Logo();
                string company = UseFul.NumberEmpresa(parametro);

                logo = (new LogoDao()).ReadId(company);
                if (logo.Name == null)
                {
                    logo = (new LogoDao()).ReadId("0");
                }

                //memoryImage = new MemoryStream(client.DownloadData("https://spiderfleetapi.azurewebsites.net/templates/logo/spider.png"));
                memoryImage = new MemoryStream(client.DownloadData("https://spiderfleetapi.azurewebsites.net/templates/logo/" + logo.Name));
                nameImage = "Spider_" + company;
                Height = 80;
                Width = 270;
                rowIndex = 0;
                colIndex = 0;
            }
            catch (Exception ex)
            {
                clientImage.Dispose();
            }
            #endregion

            bool band = false;

            MemoryStream memoryZip = new MemoryStream();
            ReporteItinerarios itinerarios = new ReporteItinerarios();

            if (type == 2)  //if (parametro.Equals("/79/"))
            {
                memoryZip = itinerarios.Reporte(2, band, tempOutput, path, fechaInicio, fechaFin, maxVelocidad,
                    memoryStream, nombreVehiculo,
                    memoryImage, nameImage, rowIndex, colIndex, Height, Width,
                    filesExcel, devices);
            }
            else if (type == 1)
            {

                string respuesta = string.Empty;

                string param2 = "/" + param.Replace("-", "/").ToString() + "/";
                respuesta = (new SettingConfig()).ReadIdHerarchy(param2, "DIR", 2);

                if (string.IsNullOrEmpty(respuesta))
                {
                    respuesta = (new SettingConfig()).ReadIdHerarchy(parametro, "DIR", 2);
                }

                //respuesta = (new SettingConfig()).ReadIdHerarchy(parametro, "DIR", 2);

                band = string.IsNullOrEmpty(respuesta) | respuesta.Equals("0") ? false : true;

                memoryZip = itinerarios.Reporte(1, band, tempOutput, path, fechaInicio, fechaFin, maxVelocidad,
                    memoryStream, nombreVehiculo,
                    memoryImage, nameImage, rowIndex, colIndex, Height, Width,
                    filesExcel, devices);
            }

            var reporte = new HttpResponseMessage(HttpStatusCode.OK);
            reporte.Content = new ByteArrayContent(memoryZip.ToArray());
            reporte.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            reporte.Content.Headers.ContentDisposition.FileName = fileName; //"Archivos.zip";
            reporte.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            return reporte;

            #endregion

        }

        [HttpGet]
        [AllowAnonymous]
        [Route("api/report/new/itinerarios/zip")]
        public async System.Threading.Tasks.Task<HttpResponseMessage> ReportesNewLogicalItinerariosZip([FromUri] int empresa, [FromUri] string param, [FromUri] int type,
            [FromUri] string grupo, [FromUri] string device,
            [FromUri] DateTime fechaInicio, [FromUri] DateTime fechaFin)
        {

            var fileName = string.Empty;
            var tempOutput = string.Empty;

            #region 

            DeviceResponse listDevice = new DeviceResponse();
            BusinessReportResponse response = new BusinessReportResponse();
            MemoryStream memoryStream = new MemoryStream();
            WebClient client = new WebClient();
            ReportItinerarioResponse information = new ReportItinerarioResponse();
            string nombreVehiculo = string.Empty;

            string parametro = use.hierarchyPrincipal(param);
            decimal maxVelocidad = Convert.ToDecimal((new SettingConfig()).ReadIdHerarchy(use.hierarchyPrincipal(param), MXV, 1));

            try
            {
                //memoryStream = new MemoryStream(client.DownloadData("C:/Simopa/Templete/TemplateItinerarios.xlsx"));

                if (type == 2)//.Equals("/79/"))
                {
                    memoryStream = new MemoryStream(client.DownloadData("https://spiderfleetapi.azurewebsites.net/templates/reportes/TemplateItinerariosPetro.xlsx"));
                }
                else if (type == 1)
                {
                    memoryStream = new MemoryStream(client.DownloadData("https://spiderfleetapi.azurewebsites.net/templates/reportes/TemplateItinerarios.xlsx"));
                }
            }
            finally
            {
                client.Dispose();
            }

            List<string> filesExcel = new List<string>();
            List<string> devices = new List<string>();

            if (!string.IsNullOrEmpty(grupo))
            {
                if (!string.IsNullOrEmpty(device))
                {
                    devices.Add(device);
                    listDevice = (new GeneralDao()).ReadGroupName(device);
                    fileName = listDevice.Grupo + ".zip";
                }
                else
                {
                    listDevice = (new GeneralDao()).ReadDeviceByGrupo(grupo);
                    devices = listDevice.ListDevice;
                    fileName = listDevice.Grupo + ".zip";
                }
            }
            else
            {
                devices.Add(device);
                listDevice = (new GeneralDao()).ReadGroupName(device);
                fileName = listDevice.Grupo + ".zip";
            }

            tempOutput = path + fileName;

            #region Logo de Empresa

            //Imagen           
            int Height = 0;
            int Width = 0;
            int rowIndex = 0;
            int colIndex = 0;
            string nameImage = string.Empty;

            WebClient clientImage = new WebClient();
            MemoryStream memoryImage = new MemoryStream();
            try
            {

                CredencialSpiderFleet.Models.Logo.Logo logo = new CredencialSpiderFleet.Models.Logo.Logo();
                string company = UseFul.NumberEmpresa(parametro);

                logo = (new LogoDao()).ReadId(company);
                if (logo.Name == null)
                {
                    logo = (new LogoDao()).ReadId("0");
                }

                //memoryImage = new MemoryStream(client.DownloadData("https://spiderfleetapi.azurewebsites.net/templates/logo/spider.png"));
                memoryImage = new MemoryStream(client.DownloadData("https://spiderfleetapi.azurewebsites.net/templates/logo/" + logo.Name));
                nameImage = "Spider_" + company;
                Height = 80;
                Width = 270;
                rowIndex = 0;
                colIndex = 0;
            }
            catch (Exception ex)
            {
                clientImage.Dispose();
            }
            #endregion

            bool band = false;

            MemoryStream memoryZip = new MemoryStream();
            ReporteItinerariosLogicalNew itinerarios = new ReporteItinerariosLogicalNew();

            if (type == 2)  //if (parametro.Equals("/79/"))
            {
                memoryZip = itinerarios.Reporte(2, band, tempOutput, path, fechaInicio, fechaFin, maxVelocidad,
                    memoryStream, nombreVehiculo,
                    memoryImage, nameImage, rowIndex, colIndex, Height, Width,
                    filesExcel, devices);
            }
            else if (type == 1)
            {

                string respuesta = string.Empty;

                string param2 = "/" + param.Replace("-", "/").ToString() + "/";
                respuesta = (new SettingConfig()).ReadIdHerarchy(param2, "DIR", 2);

                if (string.IsNullOrEmpty(respuesta))
                {
                    respuesta = (new SettingConfig()).ReadIdHerarchy(parametro, "DIR", 2);
                }

                //respuesta = (new SettingConfig()).ReadIdHerarchy(parametro, "DIR", 2);

                band = string.IsNullOrEmpty(respuesta) | respuesta.Equals("0") ? false : true;

                memoryZip = itinerarios.Reporte(1, band, tempOutput, path, fechaInicio, fechaFin, maxVelocidad,
                    memoryStream, nombreVehiculo,
                    memoryImage, nameImage, rowIndex, colIndex, Height, Width,
                    filesExcel, devices);
            }

            var reporte = new HttpResponseMessage(HttpStatusCode.OK);
            reporte.Content = new ByteArrayContent(memoryZip.ToArray());
            reporte.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            reporte.Content.Headers.ContentDisposition.FileName = fileName; //"Archivos.zip";
            reporte.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            return reporte;

            #endregion

        }


        [HttpGet]
        [AllowAnonymous]
        [Route("api/report/itinerarios/{param}/{device}")]
        public async System.Threading.Tasks.Task<HttpResponseMessage> ReportesItinerarios(string param, string device,
            [FromUri] DateTime fechaInicio, [FromUri] DateTime fechaFin)
        {
            BusinessReportResponse response = new BusinessReportResponse();
            MemoryStream memoryStream = new MemoryStream();
            WebClient client = new WebClient();
            ReportItinerarioResponse information = new ReportItinerarioResponse();
            string nombreVehiculo = string.Empty;

            try
            {
                //memoryStream = new MemoryStream(client.DownloadData("C:/Simopa/Templete/TemplateItinerarios.xlsx"));
                memoryStream = new MemoryStream(client.DownloadData("https://spiderfleetapi.azurewebsites.net/templates/reportes/TemplateItinerarios.xlsx"));
            }
            finally
            {
                client.Dispose();
            }

            

            decimal maxVelocidad = Convert.ToDecimal((new SettingConfig()).ReadIdHerarchy(use.hierarchyPrincipal(param), MXV, 1));

            MemoryStream ms = new MemoryStream();
            //Envia datos a Excel
            using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
            {

                ResponsibleVehicleResponse responsible = new ResponsibleVehicleResponse();
                responsible = (new ResponsibleDao()).ReadVehicle(device);

                if(!string.IsNullOrEmpty(responsible.responsible.Vehicle))
                {
                    nombreVehiculo = responsible.responsible.Vehicle;
                }
                else
                {
                    responsible = (new ResponsibleDao()).ReadNameVehicle(device);
                    nombreVehiculo = responsible.responsible.Vehicle;
                }


                ExcelWorkbook excelWorkBook = excelPackage.Workbook;
                ExcelWorksheet excelWorksheet = excelWorkBook.Worksheets[1];
                excelWorksheet.Workbook.CalcMode = ExcelCalcMode.Manual;


                int rows = 15;
                int columns = 2;
                int totalSegundos = 0;
                int totalMetros = 0;
                int totalFrenado = 0;
                int totalAceleracion = 0;
                double totalConsumo = 0.0;
                int totalVelocidad = 0;
                int totalRPM = 0;

                //fechaFin = fechaFin.AddDays(1);

                information = (new ReportItinerarioDao()).Read(device, fechaInicio, fechaFin, maxVelocidad);

                excelWorksheet.Cells[3, 6].Value = responsible.responsible.Vehicle;
                //fechaFin = fechaFin.AddDays(-1);
                excelWorksheet.Cells[6, 4].Value = fechaInicio.ToString("dd-MM-yyyy") + " al " + fechaFin.ToString("dd-MM-yyyy");
                excelWorksheet.Cells[7, 4].Value = responsible.responsible.Responsible;

                foreach (var data in information.itinerarios)
                {
                    excelWorksheet.Cells[rows, columns].Value = data.Viaje; columns++;
                    excelWorksheet.Cells[rows, columns].Value = data.Fecha; columns++;
                    excelWorksheet.Cells[rows, columns].Value = data.Inicio; columns++;
                    excelWorksheet.Cells[rows, columns].Value = data.Fin; columns++;

                    string tiempo = string.Empty;
                    tiempo = use.CalcularTiempo(data.Tiempo);
                    //tiempo = UseFul.CalcularTime(data.Tiempo);
                    totalSegundos = totalSegundos + (Convert.ToInt32(data.Tiempo));

                    excelWorksheet.Cells[rows, columns].Value = tiempo; columns++;

                    totalAceleracion = totalAceleracion + data.Aceleracion;

                    excelWorksheet.Cells[rows, columns].Value = data.Aceleracion; columns++;

                    totalFrenado = totalFrenado + data.Frenado;

                    excelWorksheet.Cells[rows, columns].Value = data.Frenado; columns++;

                    totalRPM = totalRPM + data.RPM;
                    excelWorksheet.Cells[rows, columns].Value = data.RPM; columns++;

                    totalVelocidad = totalVelocidad + data.Velocidad;

                    excelWorksheet.Cells[rows, columns].Value = data.Velocidad; columns++;

                    string distancia = string.Empty;
                    distancia = use.metrosKilometros(data.Distancia);
                    totalMetros = totalMetros + (data.Distancia);

                    excelWorksheet.Cells[rows, columns].Value = distancia; columns++;
                    double consumo = (Convert.ToDouble(distancia) * 1) / 10;

                    excelWorksheet.Cells[rows, columns].Value = Math.Round(consumo, 2).ToString(); columns++;//data.Consumo; columns++;

                    totalConsumo = totalConsumo + consumo;


                    DateTime timeIni = data.FechaInicio;

                    string fechaini = timeIni.ToString("yyyy-MM-dd");
                    string horaini = fechaini + "T" + data.FechaInicio.ToString("HH:mm:ss") + "Z";

                    DateTime timeFin = data.FechaFin;

                    string fechaFn = timeFin.ToString("yyyy-MM-dd");
                    string horaFn = fechaini + "T" + data.FechaFin.ToString("HH:mm:ss") + "Z";

                    ////string link = "http://spiderfleetapi.azurewebsites.net/#/trip/" + device + "/" + data.FechaInicio.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'") + "/" + data.FechaFin.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'");
                    string link = "http://spiderfleetapi.azurewebsites.net/#/trip/" + device + "/" + horaini + "/" + horaFn;

                    columns++; columns++; columns++;
                    //excelWorksheet.Cells[rows, columns].Hyperlink = new Uri(link);

                    ExcelHyperLink hl = new ExcelHyperLink(link);
                    hl.Display= "Link";

                    excelWorksheet.Cells[rows, columns].Hyperlink = hl;

                    rows++;
                    columns = 2;
                }

                excelWorksheet.Cells[8, 4].Value = Convert.ToString(Math.Round(Convert.ToDouble(use.metrosKilometros(totalMetros)) / Math.Round(totalConsumo, 2), 2)) + " Km/L";
                excelWorksheet.Cells[9, 4].Value = use.metrosKilometros(totalMetros) + " Km";
                excelWorksheet.Cells[10, 4].Value = use.CalcularTiempo(totalSegundos) + " Hrs";
                //excelWorksheet.Cells[10, 4].Value = UseFul.CalcularTime(totalSegundos) + " Hrs";
                excelWorksheet.Cells[11, 4].Value = Math.Round(totalConsumo, 2).ToString() + " Litros";

                excelWorksheet.Cells[8, 7].Value = totalAceleracion;
                excelWorksheet.Cells[8, 8].Value = totalFrenado;
                excelWorksheet.Cells[8, 9].Value = totalRPM;
                excelWorksheet.Cells[8, 10].Value = totalVelocidad;


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
                    FileName = "ReporteItinerarios_" + nombreVehiculo +".xlsx"
                };
            reporte.Content.Headers.ContentType =
                new MediaTypeHeaderValue("application/octet-stream");

            response.reporte = reporte;

            return reporte;
        }

        /// <summary>
        /// Este Reporte Esta diseñado para el antiguo Spider
        /// </summary>
        /// <param name="device"></param>
        /// <param name="inicio"></param>
        /// <param name="fin"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("api/report/data/{device}")]
        public async System.Threading.Tasks.Task<HttpResponseMessage> DescargaReporteSumarioAdquConcurso(string device, 
            [FromUri] DateTime inicio, [FromUri] DateTime fin)
        {
            BusinessReportResponse response = new BusinessReportResponse();
            MemoryStream memoryStream = new MemoryStream();
            WebClient client = new WebClient();
            ReportItinerarioResponse information = new ReportItinerarioResponse();


            try
            {
                //memoryStream = new MemoryStream(client.DownloadData("C:/Simopa/Templete/TemplateItinerarios.xlsx"));
                memoryStream = new MemoryStream(client.DownloadData("https://spiderfleetapi.azurewebsites.net/templates/reportes/TemplateItinerarios.xlsx"));
            }
            finally
            {
                client.Dispose();
            }

            decimal maxVelocidad = Convert.ToDecimal((new SettingConfig()).ReadId("SGS", "MXV", 1));


            MemoryStream ms = new MemoryStream();
            //Envia datos a Excel
            using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
            {

                ResponsibleVehicleResponse responsible = new ResponsibleVehicleResponse();
                responsible = (new ResponsibleDao()).ReadVehicle(device);

                ExcelWorkbook excelWorkBook = excelPackage.Workbook;
                ExcelWorksheet excelWorksheet = excelWorkBook.Worksheets[1];
                excelWorksheet.Workbook.CalcMode = ExcelCalcMode.Manual;


                int rows = 15;
                int columns = 2;
                int totalSegundos = 0;
                int totalMetros = 0;
                int totalFrenado = 0;
                int totalAceleracion = 0;
                double totalConsumo = 0.0;
                int totalVelocidad = 0;
                int totalRPM = 0;

                fin = fin.AddDays(1);

                information = (new ReportItinerarioDao()).Read(device, inicio, fin, maxVelocidad);

                excelWorksheet.Cells[3, 6].Value = responsible.responsible.Vehicle;
                fin = fin.AddDays(-1);
                excelWorksheet.Cells[6, 4].Value = inicio.ToString("dd-MM-yyyy") + " al " + fin.ToString("dd-MM-yyyy");
                excelWorksheet.Cells[7, 4].Value = responsible.responsible.Responsible;

                string date = fin.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                foreach (var data in information.itinerarios)
                {
                    excelWorksheet.Cells[rows, columns].Value = data.Viaje; columns++;
                    excelWorksheet.Cells[rows, columns].Value = data.Fecha; columns++;
                    excelWorksheet.Cells[rows, columns].Value = data.Inicio; columns++;
                    excelWorksheet.Cells[rows, columns].Value = data.Fin; columns++;

                    string tiempo = string.Empty;
                    tiempo = use.CalcularTiempo(data.Tiempo);
                    totalSegundos = totalSegundos + (Convert.ToInt32(data.Tiempo));

                    excelWorksheet.Cells[rows, columns].Value = tiempo; columns++;

                    totalAceleracion = totalAceleracion + data.Aceleracion;

                    excelWorksheet.Cells[rows, columns].Value = data.Aceleracion; columns++;

                    totalFrenado = totalFrenado + data.Frenado;

                    excelWorksheet.Cells[rows, columns].Value = data.Frenado; columns++;

                    totalRPM = totalRPM + data.RPM;
                    excelWorksheet.Cells[rows, columns].Value = data.RPM; columns++;

                    totalVelocidad = totalVelocidad + data.Velocidad;

                    excelWorksheet.Cells[rows, columns].Value = data.Velocidad; columns++;

                    string distancia = string.Empty;
                    distancia = use.metrosKilometros(data.Distancia);
                    totalMetros = totalMetros + (data.Distancia);

                    excelWorksheet.Cells[rows, columns].Value = distancia; columns++;
                    double consumo = (Convert.ToDouble(distancia) * 1 ) / 10;
                    
                    excelWorksheet.Cells[rows, columns].Value = Math.Round(consumo, 2).ToString(); columns++;//data.Consumo; columns++;

                    totalConsumo = totalConsumo + consumo;

                    rows++;
                    columns = 2;
                }

                excelWorksheet.Cells[8, 4].Value = Convert.ToString(Math.Round(Convert.ToDouble(use.metrosKilometros(totalMetros)) / Math.Round(totalConsumo, 2), 2)) + " Km/L";
                excelWorksheet.Cells[9, 4].Value = use.metrosKilometros(totalMetros) + " Km";
                excelWorksheet.Cells[10, 4].Value = use.CalcularTiempo(totalSegundos) + " Hrs";
                excelWorksheet.Cells[11, 4].Value = Math.Round(totalConsumo, 2).ToString() + " Litros";

                excelWorksheet.Cells[8, 7].Value = totalAceleracion;
                excelWorksheet.Cells[8, 8].Value = totalFrenado;
                excelWorksheet.Cells[8, 9].Value = totalRPM;
                excelWorksheet.Cells[8, 10].Value = totalVelocidad;


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
                    FileName = "ReporteItinerarios.xlsx"
                };
            reporte.Content.Headers.ContentType =
                new MediaTypeHeaderValue("application/octet-stream");

            response.reporte = reporte;

            return reporte;
        }

    }
}
