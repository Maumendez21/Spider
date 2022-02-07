using CredencialSpiderFleet.Models.Useful;
using SpiderFleetWebAPI.Models.Response.General;
using SpiderFleetWebAPI.Models.Response.ReportAdmin;
using SpiderFleetWebAPI.Utils.General;
using SpiderFleetWebAPI.Utils.Logo;
using SpiderFleetWebAPI.Utils.ReportAdmin;
using SpiderFleetWebAPI.Utils.Setting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Hosting;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.ReportAdmin
{
    public class ReporteItinerariosMongoController : ApiController
    {
        private UseFul use = new UseFul();
        private const string MXV = "MXV";
        private string path = HostingEnvironment.MapPath("/TempFiles/");

        [HttpGet]
        [AllowAnonymous]
        [Route("api/reporte/viajes/mongo/zip")]
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

                if (type == 2)
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
            ReporteItinerariosMongo itinerarios = new ReporteItinerariosMongo();

            if (type == 2)  //if (parametro.Equals("/79/"))
            {
                memoryZip = itinerarios.Reporte(parametro, 2, band, tempOutput, path, fechaInicio, fechaFin, maxVelocidad,
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

                respuesta = (new SettingConfig()).ReadIdHerarchy(parametro, "DIR", 2);

                band = string.IsNullOrEmpty(respuesta) | respuesta.Equals("0") ? false : true;

                memoryZip = itinerarios.Reporte(parametro, 1, band, tempOutput, path, fechaInicio, fechaFin, maxVelocidad,
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

    }
}
