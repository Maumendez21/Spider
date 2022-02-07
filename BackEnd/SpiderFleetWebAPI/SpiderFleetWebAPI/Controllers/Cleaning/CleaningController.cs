using SpiderFleetWebAPI.Models.Response.Cleaning;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Cleaning
{
    public class CleaningController : ApiController
    {
        private const string Tag = "Limpieza de Tablas GPS y Alarmas sinocastel";
        private const string BasicRoute = "api/";
        private const string ResourceName = "cleaning/";

        [HttpGet]
        [Route(BasicRoute + ResourceName + "gps/sql")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(CleaningResponse))]
        public CleaningResponse CleaningGps()
        {
            CleaningResponse response = new CleaningResponse();

            try
            {
                try
                {
                    response = new Utils.Cleaning.CleaningDao().CleaningSqlGPS();
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

        [HttpGet]
        [Route(BasicRoute + ResourceName + "alarmas/sql")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(CleaningResponse))]
        public CleaningResponse CleaningAlarmas()
        {
            CleaningResponse response = new CleaningResponse();

            try
            {
                try
                {
                    response = new Utils.Cleaning.CleaningDao().CleaningSqlAlarmas();
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


        [HttpGet]
        [Route(BasicRoute + ResourceName + "gps/mongo")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(CleaningResponse))]
        public CleaningResponse CleaningGpsMongo()
        {
            CleaningResponse response = new CleaningResponse();

            try
            {
                try
                {
                    response = new Utils.Cleaning.CleaningDao().CleaningMongoGPS();
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

        [HttpGet]
        [Route(BasicRoute + ResourceName + "alarmas/mongo")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(CleaningResponse))]
        public CleaningResponse CleaningAlarmasMongo()
        {
            CleaningResponse response = new CleaningResponse();

            try
            {
                try
                {
                    response = new Utils.Cleaning.CleaningDao().CleaningMongoAlarmas();
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
