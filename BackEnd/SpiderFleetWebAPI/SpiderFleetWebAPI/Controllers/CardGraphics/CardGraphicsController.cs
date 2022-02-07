using SpiderFleetWebAPI.Models.Response.CardGraphics;
using SpiderFleetWebAPI.Utils.CardGraphics;
using SpiderFleetWebAPI.Utils.User;
using SpiderFleetWebAPI.Utils.VerifyUser;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.CardGraphics
{
    public class CardGraphicsController : ApiController
    {

        private const string Tag = "Graficas Tarjeta";
        private const string BasicRoute = "api/";
        private const string ResourceName = "card/graphics";


        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName + "/odo")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(CardGraphicsResponse))]
        public CardGraphicsResponse ReadTripsFiveDevice([FromUri(Name = "device")] string device)
        {
            CardGraphicsResponse response = new CardGraphicsResponse();

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
                    response = (new CardGraphicsDao()).Graphics(hierarchy, device);
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
