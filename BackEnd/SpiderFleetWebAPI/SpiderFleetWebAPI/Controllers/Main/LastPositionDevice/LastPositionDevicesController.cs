using CredencialSpiderFleet.Models.Main.LastPositionDevice;
using SpiderFleetWebAPI.Models.Response.Main.LastPositionDevice;
using SpiderFleetWebAPI.Utils.Main.LastPositionDevice;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Main.LastPositionDevice
{
    public class LastPositionDevicesController : ApiController
    {
        private const string Tag = "Listado de Ultima posicion de Dispositivos";
        private const string BasicRoute = "api/";
        private const string ResourceName = "spider";


        [HttpGet]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(LastPositionDevicesResponse))]
        public IHttpActionResult GetListLastPositionDevice([FromUri(Name = "tipo")]string tipo, [FromUri(Name = "valor")]string valor)
        {
            LastPositionDevicesResponse response = new LastPositionDevicesResponse();
            try
            {
                try
                {
                    LastPositionDevices position = new LastPositionDevices();
                    List<LastPositionDevices> listPosition = (new LastPositionDevicesDao()).ReadLastPositionDevice(tipo, valor);

                    return Ok(listPosition);
                }
                catch (Exception ex)
                {
                    response.success = false;
                    response.messages.Add(ex.Message);
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return Ok(response);
            }
        }
    }
}
