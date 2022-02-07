using SpiderFleetWebAPI.Models.Response.Mobility.InfoResponsibles;
using SpiderFleetWebAPI.Utils.Mobility.InfoResponsibles;
using SpiderFleetWebAPI.Utils.User;
using SpiderFleetWebAPI.Utils.VerifyUser;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Mobility.InfoResponsibles
{
    public class InfoResponsiblesController : ApiController
    {
        private const string Tag = "Lista de Responsables Agendados";
        private const string BasicRoute = "api/";
        private const string ResourceName = "diary/responsibles";

        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName + "/list")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(InfoResponsiblesResponse))]
        public async Task<InfoResponsiblesResponse> GetAllAlarms([FromUri(Name = "device")] string device,
            [FromUri(Name = "start")] DateTime startdate, [FromUri(Name = "end")] DateTime enddate)
        {
            InfoResponsiblesResponse response = new InfoResponsiblesResponse();
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
                    response = await (new InfoResponsiblesDao()).GetAllResponsibles(device, startdate, enddate, hierarchy);
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
