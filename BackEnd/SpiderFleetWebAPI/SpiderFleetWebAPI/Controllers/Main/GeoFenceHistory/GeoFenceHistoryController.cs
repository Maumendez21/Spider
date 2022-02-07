using SpiderFleetWebAPI.Models.Response.Main.GeoFenceHistory;
using SpiderFleetWebAPI.Utils.Main.GeoFenceHistory;
using SpiderFleetWebAPI.Utils.User;
using SpiderFleetWebAPI.Utils.VerifyUser;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Main.GeoFenceHistory
{
    public class GeoFenceHistoryController : ApiController
    {

        private const string Tag = "Historico Fuera de GeoCerca ";
        private const string BasicRoute = "api/";
        private const string ResourceName = "geo/fence/history";


        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName + "/list")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(GeoFenceHistoryResponse))]
        public GeoFenceHistoryResponse GetPointTimeOut([FromUri(Name = "device")] string device, [FromUri(Name = "mongo")] string mongo, 
            [FromUri(Name = "start")] DateTime start, [FromUri(Name = "end")] DateTime end)
        {
            GeoFenceHistoryResponse response = new GeoFenceHistoryResponse();

            string hierarchy = string.Empty;

            try
            {
                string username = string.Empty;
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
                response = (new GeoFenceHistoryDao()).GetTimeOutDevice(device, mongo, start, end);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }

            return response;
        }
    }
}
