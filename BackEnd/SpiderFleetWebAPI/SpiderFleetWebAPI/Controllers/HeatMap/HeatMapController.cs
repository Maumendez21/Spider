using SpiderFleetWebAPI.Models.Response.HeatMap;
using SpiderFleetWebAPI.Utils.HeatMap;
using SpiderFleetWebAPI.Utils.User;
using SpiderFleetWebAPI.Utils.VerifyUser;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Net;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.HeatMap
{
    public class HeatMapController : ApiController
    {
        private const string Tag = "Mapa de Calor";
        private const string BasicRoute = "api/";
        private const string ResourceName = "heat/map";



        ///BackLog - 874 Task - 953
        /// <summary>
        /// Mapa de Calor por Rango de Fechas, Grupo y Dispositivo
        /// </summary>
        /// <remarks>
        /// <param name="startdate">2020-08-25</param>
        /// <param name="enddate">2020-11-25</param>
        /// <param name="group">/72/</param>
        /// <param name="device">213WP2018002078</param>
        /// <returns>success = true o false;</returns>
        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName )]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(HeatMapResponse))]
        public HeatMapResponse ReadHeatMap(
            [FromUri(Name = "startdate")] DateTime startdate, [FromUri(Name = "enddate")] DateTime enddate,
            [FromUri(Name = "group")] string group, [FromUri(Name = "device")] string device)
        {
            HeatMapResponse response = new HeatMapResponse();

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
                    response = (new HeatMapDao()).Coordenates(hierarchy, group, startdate, enddate, device);
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
