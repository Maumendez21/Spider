using CredencialSpiderFleet.Models.Useful;
using SpiderFleetWebAPI.Models.Response.DashBoard;
using SpiderFleetWebAPI.Models.Response.Main.LastPositionDevice;
using SpiderFleetWebAPI.Utils.DashBoard;
using SpiderFleetWebAPI.Utils.Logo;
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
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Dashboard
{
    public class DashBoardActivityDayController : ApiController
    {
        private const string Tag = "DashBoard del Dia";
        private const string BasicRoute = "api/";
        private const string ResourceName = "dashboard/activity/day";

        private string path = HostingEnvironment.MapPath("/TempFiles/");

        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName + "/list/devices")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(DashBoardActivityDayResponse))]
        public async Task<DashBoardActivityDayResponse> GetDashBoarDay([FromUri(Name = "busqueda")] string busqueda)
        {
            DashBoardActivityDayResponse response = new DashBoardActivityDayResponse();

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
                    
                    response = await (new DashBoardActivityDayDao()).DashBoardDay(hierarchy, busqueda);
                    
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

        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName + "/list/notifications")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(NotificationsResponse))]
        public NotificationsResponse GetNotifications([FromUri(Name = "start")] DateTime start, [FromUri(Name = "end")] DateTime end)
        {
            NotificationsResponse response = new NotificationsResponse();
            try
            {

                string hierarchy = string.Empty;
                string username = string.Empty;
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
                    response = (new DashBoardActivityDayDao()).NotificationsPriority(hierarchy, start, end);
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

        //Excel

        [HttpGet]
        [AllowAnonymous]
        [Route("api/dashboard/activity/day/report/analitica")]
        public async System.Threading.Tasks.Task<HttpResponseMessage> ReportesItinerariosZip([FromUri] string param,
                //[FromUri] string grupo, [FromUri] string device,
                [FromUri] DateTime fechaInicio, [FromUri] DateTime fechaFin)
        {

            var tempOutput = string.Empty;

            #region 

            MemoryStream memoryStream = new MemoryStream();
            WebClient client = new WebClient();

            UseFul use = new UseFul();
            string parametro = use.hierarchyPrincipal(param);
            string node = use.getNode(param);


            try
            {
                memoryStream = new MemoryStream(client.DownloadData("https://spiderfleetapi.azurewebsites.net/templates/reportes/TemplateAnaliticaNotificaciones.xlsx"));
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

            memoryZip = (new DashBoardActivityDayDao()).Reporte(node, 
                //grupo, device, 
                fechaInicio, fechaFin,
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
