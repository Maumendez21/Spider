using SpiderFleetWebAPI.Models.Response.ReportAdmin;
using SpiderFleetWebAPI.Utils.ReportAdmin;
using SpiderFleetWebAPI.Utils.VerifyUser;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Net;
using System.Web.Http;


namespace SpiderFleetWebAPI.Controllers.ReportAdmin
{
    public class LastEventController : ApiController
    {
        private const string Tag = " Ultimo Evento";
        private const string BasicRoute = "api/";
        private const string ResourceName = "list/";

        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName + "events")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(LastEventResponse))]
        public LastEventResponse GetVehicles([FromUri(Name = "idempresa")] string idempresa, [FromUri(Name = "events")] string events)
        {
            LastEventResponse response = new LastEventResponse();

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

                response = (new LastEventDao()).ReadLastEvent(idempresa, events);

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
