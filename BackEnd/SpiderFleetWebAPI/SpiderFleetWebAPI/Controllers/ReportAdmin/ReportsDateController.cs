using SpiderFleetWebAPI.Models.Response.ReportAdmin;
using SpiderFleetWebAPI.Utils.ReportAdmin;
using SpiderFleetWebAPI.Utils.VerifyUser;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Net;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.ReportAdmin
{
    public class ReportsDateController : ApiController
    {
        private const string Tag = " Reportes ";
        private const string BasicRoute = "api/";
        private const string ResourceName = "reports/date/";
        private const string format = "dd/MM/yyyy HH:mm:ss tt";

        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName + "gps")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ReportGPSResponse))]
        public ReportGPSResponse GetVehicles([FromUri(Name = "start")] string start, [FromUri(Name = "end")] string end, [FromUri(Name = "device")] string device)
        {
            ReportGPSResponse response = new ReportGPSResponse();
            try
            {
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

                DateTime ini;
                DateTime fin;
                    
                DateTime.TryParse(start, out ini);
                DateTime.TryParse(end, out fin);

                response = (new ReportGpsDao()).ReadGps(ini, fin, device);

                return response;
            }
            catch(Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }
        }
    }
}
