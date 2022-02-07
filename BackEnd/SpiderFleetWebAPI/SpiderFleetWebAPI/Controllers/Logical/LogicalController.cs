using SpiderFleetWebAPI.Models.Response.Logical;
using SpiderFleetWebAPI.Utils.Logical;
using SpiderFleetWebAPI.Utils.VerifyUser;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Logical
{
    public class LogicalController : ApiController
    {
        private const string Tag = "Raw Data de Viajes del Dispositivo";
        private const string BasicRoute = "api/";
        private const string ResourceName = "administration/raw/data";

        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName + "/list")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(LogicalResponse))]
        public LogicalResponse GetAllRawData([FromUri(Name = "company")] string company, [FromUri(Name = "device")] string device,
            [FromUri(Name = "startdate")] string startdate, [FromUri(Name = "enddate")] string enddate)
        {
            LogicalResponse response = new LogicalResponse();
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
                    response = (new LogicalDao()).GetRawData(company, device, startdate, enddate);
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
