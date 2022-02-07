using SpiderFleetWebAPI.Models.Response.Main.VehicleList;
using SpiderFleetWebAPI.Utils.Main.VehicleList;
using SpiderFleetWebAPI.Utils.VerifyUser;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Main.VehicleList
{
    public class VehicleListController : ApiController
    {

        private const string Tag = " Listado de Vehiculos";
        private const string BasicRoute = "api/";
        private const string ResourceName = "list/";

        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName + "vehiculos")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(VehicleListResponse))]
        public VehicleListResponse GetVehicles([FromUri(Name = "idempresa")] string idempresa)
        {
            VehicleListResponse response = new VehicleListResponse();

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

            response = (new VehicleListDao()).ReadVehicleList(idempresa);

            return response;
        }
    }
}
