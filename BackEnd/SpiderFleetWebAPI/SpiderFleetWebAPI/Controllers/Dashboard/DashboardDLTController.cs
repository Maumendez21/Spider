using CredencialSpiderFleet.Models.Useful;
using SpiderFleetWebAPI.Models.Response.DashBoard;
using SpiderFleetWebAPI.Utils.DashBoard;
using SpiderFleetWebAPI.Utils.General;
using SpiderFleetWebAPI.Utils.Logo;
using SpiderFleetWebAPI.Utils.Setting;
using SpiderFleetWebAPI.Utils.User;
using SpiderFleetWebAPI.Utils.VerifyUser;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Hosting;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Dashboard
{
    public class DashboardDLTController : ApiController
    {
        private const string Tag = "Dashboard General Distancia, Litros y Tiempo";
        private const string BasicRoute = "api/";
        private const string ResourceName = "dashboard";

        private const string MXV = "MXV";
        private string path = HostingEnvironment.MapPath("/TempFiles/");


        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName + "/distancia/litros/tiempo")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(DashBoardDLTGraficaBarraResponse))]
        public DashBoardDLTGraficaBarraResponse ReadItinerariesDevice(
            [FromUri(Name = "fechainicio")] DateTime fechainicio, [FromUri(Name = "fechafin")] DateTime fechafin,
            [FromUri(Name = "grupo")] string grupo, [FromUri(Name = "device")] string device)
        {
            DashBoardDLTGraficaBarraResponse response = new DashBoardDLTGraficaBarraResponse();

            try
            {
                string username = string.Empty;
                string hierarchy = string.Empty;

                try
                {
                    username = (new VerifyUser()).verifyTokenUser(User);
                    hierarchy = (new UserDao()).ReadUserHierarchy(username);
                }
                catch (Exception ex)
                {
                    response.success = false;
                    response.messages.Add(ex.Message);
                    return response;
                }

                try
                {
                    response = (new DashBoardDLTDao()).GetGraficas(hierarchy , fechainicio, fechafin, grupo, device);
                }
                catch (Exception ex)
                {
                    response.success = false;
                    response.messages.Add(ex.Message);
                    return response;
                }

                return response;
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("api/dashboard/report/analitica")]
        public async System.Threading.Tasks.Task<HttpResponseMessage> ReportesItinerariosZip([FromUri] string param, 
                [FromUri] string grupo, [FromUri] string device,
                [FromUri] DateTime fechaInicio, [FromUri] DateTime fechaFin)
        {

            var tempOutput = string.Empty;

            #region 

            MemoryStream memoryStream = new MemoryStream();
            WebClient client = new WebClient();
            
            UseFul use = new UseFul();
            string parametro = use.hierarchyPrincipal(param);

            try
            {
                memoryStream = new MemoryStream(client.DownloadData("https://spiderfleetapi.azurewebsites.net/templates/reportes/TemplateAnalitica.xlsx"));
            }
            finally
            {
                client.Dispose();
            }

            List<string> filesExcel = new List<string>();
            string extension = ".zip";
            var fileName = $"{Guid.NewGuid()}{extension}";
            
            tempOutput = path + "ExcelAnalitica_" + fileName;

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

            MemoryStream memoryZip = new MemoryStream();

            memoryZip = (new DashBoardDLTDao()).Reporte(parametro, grupo, device, fechaInicio, fechaFin,
                        tempOutput, path,
                        memoryStream, memoryImage, nameImage, rowIndex, colIndex, Height, Width,
                        filesExcel);
                
            var reporte = new HttpResponseMessage(HttpStatusCode.OK);
            reporte.Content = new ByteArrayContent(memoryZip.ToArray());
            reporte.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            reporte.Content.Headers.ContentDisposition.FileName = fileName;
            reporte.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            return reporte;

            #endregion

        }
    }
}
