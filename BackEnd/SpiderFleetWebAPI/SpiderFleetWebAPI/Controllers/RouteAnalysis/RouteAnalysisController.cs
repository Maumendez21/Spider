using SpiderFleetWebAPI.Models.Request.RouteAnalysis;
using SpiderFleetWebAPI.Models.Response.RouteAnalysis;
using SpiderFleetWebAPI.Utils.RouteAnalysis;
using SpiderFleetWebAPI.Utils.User;
using SpiderFleetWebAPI.Utils.VerifyUser;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Net;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.RouteAnalysis
{
    public class RouteAnalysisController : ApiController
    {

        private const string Tag = "Analisis de Rutas";
        private const string BasicRoute = "api/";
        private const string ResourceName = "routes/analysis";

        [Authorize]
        [HttpPost]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(RouteAnalysisResponse))]
        public RouteAnalysisResponse Create([FromBody] RouteAnalysisRequest routeRequest)
        {
            RouteAnalysisResponse response = new RouteAnalysisResponse();

            try
            {
                if (!(routeRequest is RouteAnalysisRequest))
                {
                    response.success = false;
                    response.messages.Add("Objeto de entrada invalido");
                    return response;
                }

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

                CredencialSpiderFleet.Models.RouteAnalysis.RouteAnalysis analysis = new CredencialSpiderFleet.Models.RouteAnalysis.RouteAnalysis();
                analysis.Id = routeRequest.Id;
                analysis.Device = routeRequest.Device;
                analysis.Coordinates = routeRequest.Coordinates;

                try
                {
                    response = (new RouteAnalysisDao()).CheckGeoFence(analysis);
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
