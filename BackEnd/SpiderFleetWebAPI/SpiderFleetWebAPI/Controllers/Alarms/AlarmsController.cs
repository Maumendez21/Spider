using SpiderFleetWebAPI.Models.Response.Alarm;
using SpiderFleetWebAPI.Utils.Alarms;
using SpiderFleetWebAPI.Utils.VerifyUser;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Alarms
{
    public class AlarmsController : ApiController
    {
        private const string Tag = "Alarmas de Dispositivo";
        private const string BasicRoute = "api/";
        private const string ResourceName = "administration/alarms";

        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName + "/list")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(AlarmResponse))]
        public AlarmResponse GetAllAlarms([FromUri(Name = "device")] string device,
            [FromUri(Name = "startdate")] DateTime startdate, [FromUri(Name = "enddate")] DateTime enddate)
        {
            AlarmResponse response = new AlarmResponse();
            try
            {

                try
                {
                    string username = string.Empty;
                    username = (new VerifyUser()).verifyTokenUser(User);
                }
                catch (Exception ex)
                {
                    response.success = false;
                    response.messages.Add(ex.Message);
                    return response;
                }

                try
                {
                    response = (new AlarmDao()).GetAllAlarms(device, startdate, enddate);
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
        [Route(BasicRoute + ResourceName + "/devices")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(DeviceDataResponse))]
        public DeviceDataResponse GetAllDevices([FromUri(Name = "company")] string company)
        {
            DeviceDataResponse response = new DeviceDataResponse();
            try
            {

                try
                {
                    string username = string.Empty;
                    username = (new VerifyUser()).verifyTokenUser(User);
                }
                catch (Exception ex)
                {
                    response.success = false;
                    response.messages.Add(ex.Message);
                    return response;
                }

                try
                {
                    response = (new AlarmDao()).GetAllDevices(company);
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
    }
}
