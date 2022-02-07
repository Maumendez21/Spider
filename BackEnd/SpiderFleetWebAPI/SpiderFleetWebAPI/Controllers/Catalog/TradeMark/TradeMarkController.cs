using SpiderFleetWebAPI.Models.Response.Catalog.TradeMark;
using SpiderFleetWebAPI.Utils.Catalog.TradeMark;
using SpiderFleetWebAPI.Utils.VerifyUser;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Net;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Catalog.TradeMark
{
    public class TradeMarkController : ApiController
    {

        private const string Tag = "Mantenimiento de Marcas de Vehiculos";
        private const string BasicRoute = "api/";
        private const string ResourceName = "administration/trade/marks";

        /// <summary>
        /// Lista de Marcas de Autos
        /// </summary>
        /// <remarks>
        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName + "/list")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(TradeMarkListResponse))]
        public TradeMarkListResponse GetListAsync()
        {
            TradeMarkListResponse response = new TradeMarkListResponse();
            try
            {
                string username = string.Empty;
                //string hierarchy = string.Empty;

                try
                {
                    username = (new VerifyUser()).verifyTokenUser(User);
                    //hierarchy = (new UserDao()).ReadUserHierarchy(username);
                }
                catch (Exception ex)
                {
                    response.success = false;
                    response.messages.Add(ex.Message);
                    return response;
                }

                try
                {
                    response = (new TradeMarkDao()).Read();
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
