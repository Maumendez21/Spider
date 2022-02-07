using SpiderFleetWebAPI.Models.Response.Catalog.SamplingTime;
using SpiderFleetWebAPI.Utils.Catalog.SamplingTime;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Catalog.SamplingTime
{
    public class SamplingTimeController : ApiController
    {
        private const string Tag = "Mantenimiento de Tiempo de Muestreo";
        private const string BasicRoute = "api/";
        private const string ResourceName = "configuration/sampling/time";

        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName + "/list")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(SamplingTimeListResponse))]
        public SamplingTimeListResponse GetListAsync()
        {
            SamplingTimeListResponse response = new SamplingTimeListResponse();
            try
            {
                try
                {
                    response = (new SamplingTimeDao()).Read();
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
