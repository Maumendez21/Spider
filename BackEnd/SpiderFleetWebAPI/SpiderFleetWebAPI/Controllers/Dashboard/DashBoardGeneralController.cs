using SpiderFleetWebAPI.Models.Response.DashBoard;
using SpiderFleetWebAPI.Utils.DashBoard;
using SpiderFleetWebAPI.Utils.User;
using SpiderFleetWebAPI.Utils.VerifyUser;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Dashboard
{
    public class DashBoardGeneralController : ApiController
    {
        private const string Tag = "Dashboard General";
        private const string BasicRoute = "api/";
        private const string ResourceName = "dashboard/general";


        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName )]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(DashBoardGeneralResponse))]
        public DashBoardGeneralResponse ReadDashBoradGeneral([FromUri(Name = "valor")] string valor)
        {
            DashBoardGeneralResponse response = new DashBoardGeneralResponse();

            try
            {
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

                try
                {
                    response = (new DashBoardGeneralDao()).ReadData(hierarchy, valor);
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
        [Route(BasicRoute + ResourceName + "/consumption")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(DashBoardGeneralVehicleConsumptionResponse))]
        public DashBoardGeneralVehicleConsumptionResponse ReadConsumption([FromUri(Name = "group")] string group, [FromUri(Name = "device")] string device)
        {
            DashBoardGeneralVehicleConsumptionResponse response = new DashBoardGeneralVehicleConsumptionResponse();

            try
            {
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

                try
                {
                    response = (new DashBoardGeneralDao()).ReadDataConsumption(hierarchy, group, device);
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
