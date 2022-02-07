using SpiderFleetWebAPI.Models.Response.Mobility.PointInterestAnalysis;
using SpiderFleetWebAPI.Utils.Mobility.PointInterestAnalysis;
using SpiderFleetWebAPI.Utils.VerifyUser;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Mobility.PointInterestAnalysis
{
    public class PointInterestAnalysisController : ApiController
    {
        private const string Tag = "Lista de Responsables Agendados";
        private const string BasicRoute = "api/";
        private const string ResourceName = "point/interest/analysis";

        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName + "/list")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(PointInterestAnalysisResponse))]
        public PointInterestAnalysisResponse GetAllAlarms([FromUri(Name = "mongo")] string mongo, [FromUri(Name = "device")] string device,
            [FromUri(Name = "start")] DateTime startdate, [FromUri(Name = "end")] DateTime enddate)
        {
            PointInterestAnalysisResponse response = new PointInterestAnalysisResponse();
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
                    response = (new PointInterestAnalysisDao()).Analysis(mongo, startdate, enddate, device);
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
