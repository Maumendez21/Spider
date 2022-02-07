using CredencialSpiderFleet.Models.ReportAdmin;
using SpiderFleetWebAPI.Models.Response.ReportAdmin;
using SpiderFleetWebAPI.Utils.ReportAdmin;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.ReportAdmin
{
    public class ReportLastStatusDeviceController : ApiController
    {
        private const string Tag = " Ultimo Status ";
        private const string BasicRoute = "api/";
        private const string ResourceName = "status/device";

        //[Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName + "/last/data")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ReportLastStatusDeviceResponse))]
        public ReportLastStatusDeviceResponse GetVehicles([FromUri(Name = "device")] string device)
        {
            ReportLastStatusDeviceResponse response = new ReportLastStatusDeviceResponse();

            try
            {
                ReportLastStatusDevice statusDevice = new ReportLastStatusDevice();
                List<ReportLastStatusAlarms> listAlarms = new List<ReportLastStatusAlarms>();

                statusDevice = (new ReportLastStatusDao()).ReadLastStatusDevice(device);
                listAlarms = (new ReportLastStatusDao()).ReadLastStatusAlarms(device);

                response.statusDevice = statusDevice;
                response.listAlarms = listAlarms;
                response.success = true;
                return response;
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }
        }
    }
}
