using CredencialSpiderFleet.Models.Models.Mongo.GeoFence;
using SpiderFleetWebAPI.Models.Request.Main.GeoFence;
using SpiderFleetWebAPI.Models.Response.Main.GeoFence;
using SpiderFleetWebAPI.Utils.Main.GeoFence;
using SpiderFleetWebAPI.Utils.User;
using SpiderFleetWebAPI.Utils.VerifyUser;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Main.GeoFence
{
    public class GeoFenceController : ApiController
    {
        private const string Tag = "Geo Cercas";
        private const string BasicRoute = "api/";
        private const string ResourceName = "geo/fence";

        /// <summary>
        /// Creacion de GeoCerca
        /// </summary>
        /// <remarks>
        /// Este EndPoint que actualiza el registro 
        /// #### Ejemplo de entrada
        /// ```
        /// {
        ///"Name": "Geocerca Reforma",
        ///"Coordinates": "[[-98.3208336514726, 19.0658989489495],[-98.3100189847245, 19.0744165733859]]",
        /// }
        /// ```
        /// </remarks>
        /// <example>
        /// ```
        /// {
        ///"Name": "string",
        ///"Coordinates": "[[double, double],[double, double]]",
        /// }
        /// ```
        /// </example>
        /// <param name="geoFence">Objeto de entrada para el Endpoint</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(GeoFenceResponse))]
        public GeoFenceResponse CreateGeoFence(
           [FromBody] GeoFenceRequest geoFence)
        {
            GeoFenceResponse response = new GeoFenceResponse();

            string hierarchy = string.Empty;

            try
            {
                string username = string.Empty;
                username = (new VerifyUser()).verifyTokenUser(User);
                hierarchy = (new UserDao()).ReadUserHierarchy(username);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }

            //if (!(geoFence is GeoFenceRequest))
            //{
            //    response.success = false;
            //    response.messages.Add("Objeto de entrada invalido");
            //    return response;
            //}

            try
            {
                List<List<List<double>>> listMain = new List<List<List<double>>>();
                Polygon polygon = new Polygon();
                listMain.Add(geoFence.Coordinates);

                polygon.Type = "Polygon";
                polygon.Coordinates = listMain;

                Models.Mongo.GeoFence.GeoFence geoFences = new Models.Mongo.GeoFence.GeoFence();
                geoFences.Name = geoFence.Name;
                geoFences.Hierarchy = geoFence.Hierarchy;
                geoFences.Active = geoFence.Active;
                geoFences.Description = geoFence.Description;
                geoFences.Polygon = polygon;

                response = (new GeoFenceDao()).Create(geoFences);//, hierarchy);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }

            return response;
        }

        /// <summary>
        /// Actualización de GeoCerca
        /// </summary>
        /// <remarks>
        /// Este EndPoint que actualiza el registro 
        /// #### Ejemplo de entrada
        /// ```
        /// {
        ///"Id": "5f9d95d26ca9e80d4c43a4f9",
        ///"Name": "Geocerca Reforma",
        ///"Coordinates": "[[-98.3208336514726, 19.0658989489495],[-98.3100189847245, 19.0744165733859]]",
        /// }
        /// ```
        /// </remarks>
        /// <example>
        /// ```
        /// {
        ///"Id": "string",
        ///"Name": "string",
        ///"Coordinates": "[[double, double],[double, double]]",
        /// }
        /// ```
        /// </example>
        /// <param name="geoFence">Objeto de entrada para el Endpoint</param>
        /// <returns></returns>
        [Authorize]
        [HttpPut]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(GeoFenceResponse))]
        public GeoFenceResponse UpdateGeoFence(
           [FromBody] GeoFenceIdRequest geoFence)
        {
            GeoFenceResponse response = new GeoFenceResponse();

            string hierarchy = string.Empty;

            try
            {
                string username = string.Empty;
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
                List<List<List<double>>> listMain = new List<List<List<double>>>();
                Polygon polygon = new Polygon();
                listMain.Add(geoFence.Coordinates);

                polygon.Type = "Polygon";
                polygon.Coordinates = listMain;

                Models.Mongo.GeoFence.GeoFence geoFences = new Models.Mongo.GeoFence.GeoFence();
                geoFences.Id = geoFence.Id;
                geoFences.Name = geoFence.Name;
                geoFences.Hierarchy = geoFence.Hierarchy;
                geoFences.Active = geoFence.Active;
                geoFences.Description = geoFence.Description;
                geoFences.Polygon = polygon;

                response = (new GeoFenceDao()).Update(geoFences);//, hierarchy);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }

            return response;
        }

        /// <summary>
        /// Obtiene la lista de GeoCercas relacionadas los dispositivos que se encuentran regsitrados
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName + "/list")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(GeoFenceListHierarchyResponse))]
        public GeoFenceListHierarchyResponse GetGeoFence()
        {
            GeoFenceListHierarchyResponse response = new GeoFenceListHierarchyResponse();

            string hierarchy = string.Empty;

            try
            {
                string username = string.Empty;
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
                response = (new GeoFenceDao()).Read(hierarchy);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }

            return response;
        }

        /// <summary>
        /// Obtiene la informacion de una GeoCercas 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(BasicRoute + ResourceName )]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(GeoFenceResponse))]
        public GeoFenceRegistryResponse GetIdGeoFence([FromUri(Name = "id")] string id)
        {
            GeoFenceRegistryResponse response = new GeoFenceRegistryResponse();

             string hierarchy = string.Empty;

            try
            {
                string username = string.Empty;
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
                response = (new GeoFenceDao()).Read(id,hierarchy);
            }
            catch(Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }
            return response;
        }

        /// <summary>
        /// Eliminacion de GeoCerca y la relacion de dispositivos que se encuentran en ella
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(GeoFenceResponse))]
        public GeoFenceDeleteResponse DeleteIdGeoFence([FromUri(Name = "id")] string id)
        {
            GeoFenceDeleteResponse response = new GeoFenceDeleteResponse();

            string hierarchy = string.Empty;
            
            try
            {
                string username = string.Empty;
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
                response = (new GeoFenceDao()).Delete(hierarchy, id);
            }
            catch(Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }

            return response;
        }
    }
}