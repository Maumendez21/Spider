using SpiderFleetWebAPI.Models.Response.Sims;
using SpiderFleetWebAPI.Utils.Sims;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Net;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Sims
{
    public class SimsController : ApiController
    {
        private const string Tag = "Sims ";
        private const string BasicRoute = "api/";
        private const string ResourceName = "sims";

        [HttpGet]
        [Route(BasicRoute + ResourceName + "/available")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(SimsResponse))]
        public SimsResponse Bulking()
        {
            SimsResponse response = new SimsResponse();

            try
            {
                try
                {
                    response = (new SimsDao()).ReadAvailable();
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
