using MongoDB.Bson;
using SpiderFleetWebAPI.Models.Mongo.Route;
using SpiderFleetWebAPI.Models.Mongo.RouteConsola;
using SpiderFleetWebAPI.Models.Request.Routes;
using SpiderFleetWebAPI.Models.Response.Route;
using SpiderFleetWebAPI.Utils.Route;
using SpiderFleetWebAPI.Utils.User;
using SpiderFleetWebAPI.Utils.VerifyUser;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Route
{
    public class RoutesController : ApiController
    {
        private const string Tag = "Rutas";
        private const string BasicRoute = "api/";
        private const string ResourceName = "routes";

        [Authorize]
        [HttpPost]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(RouteResponse))]
        public RouteResponse Create([FromBody] RoutesRequest routeRequest)
        {
            RouteResponse response = new RouteResponse();

            try
            {
                if (!(routeRequest is RoutesRequest))
                {
                    response.success = false;
                    response.messages.Add("Objeto de entrada invalido");
                    return response;
                }

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

                Models.Mongo.Route.Route routes = new Models.Mongo.Route.Route();
                routes.Name = routeRequest.Name;
                routes.Description = routeRequest.Description;
                routes.Active = true;

                CredencialSpiderFleet.Models.Models.Mongo.GeoFence.Polygon Polygon = new CredencialSpiderFleet.Models.Models.Mongo.GeoFence.Polygon();
                Polygon.Type = "LineString";
                Polygon.Coordinates = new List<List<List<double>>>();
                List<List<List<double>>> listMain = new List<List<List<double>>>();
                listMain.Add(routeRequest.ListPoints);
                Polygon.Coordinates = listMain;
                routes.Polygon = Polygon;

                try
                {
                    response = (new RouteDao()).Create(routes, hierarchy);
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
        [HttpPut]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(RouteResponse))]
        public RouteResponse Update([FromBody] RoutesUpdateRequest routeRequest)
        {
            RouteResponse response = new RouteResponse();

            try
            {
                if (!(routeRequest is RoutesUpdateRequest))
                {
                    response.success = false;
                    response.messages.Add("Objeto de entrada invalido");
                    return response;
                }

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

                Models.Mongo.Route.Route routes = new Models.Mongo.Route.Route();
                routes.Id = routeRequest.Id;
                routes.Name = routeRequest.Name;
                routes.Description = routeRequest.Description;
                routes.Active = routeRequest.Active;

                CredencialSpiderFleet.Models.Models.Mongo.GeoFence.Polygon Polygon = new CredencialSpiderFleet.Models.Models.Mongo.GeoFence.Polygon();
                Polygon.Type = "LineString";
                Polygon.Coordinates = new List<List<List<double>>>();
                List<List<List<double>>> listMain = new List<List<List<double>>>();
                listMain.Add(routeRequest.ListPoints);
                Polygon.Coordinates = listMain;
                routes.Polygon = Polygon;

                try
                {
                    response = (new RouteDao()).Update(routes, hierarchy);
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
        [Route(BasicRoute + ResourceName + "/list")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(RoutesListResponse))]
        public RoutesListResponse GetRoutes()
        {
            RoutesListResponse response = new RoutesListResponse();

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
                response = (new RouteDao()).Read(hierarchy);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }

            return response;
        }

        [Authorize]
        [HttpGet]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(RouteRegistryResponse))]
        public RouteRegistryResponse GetIdRoute([FromUri(Name = "id")] string id)
        {
            RouteRegistryResponse response = new RouteRegistryResponse();

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
                response = (new RouteDao()).ReadId(id);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }
            return response;
        }

        [Authorize]
        [HttpDelete]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(RouteResponse))]
        public RouteResponse DeleteIdRoute([FromUri(Name = "id")] string id)
        {
            RouteResponse response = new RouteResponse();

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
                response = (new RouteDao()).DeleteId(id);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }
            return response;
        }

        //[Authorize]
        //[HttpPost]
        //[Route(BasicRoute + ResourceName + "/consola")]
        //[SwaggerOperation(Tags = new[] { Tag })]
        //[SwaggerResponse(HttpStatusCode.OK, Type = typeof(RouteResponse))]
        //public RouteResponse CreateConsola([FromBody] RoutesConsolaRequest request)
        //{
        //    RouteResponse response = new RouteResponse();

        //    try
        //    {
        //        if (!(request is RoutesConsolaRequest))
        //        {
        //            response.success = false;
        //            response.messages.Add("Objeto de entrada invalido");
        //            return response;
        //        }

        //        string username = string.Empty;
        //        string hierarchy = string.Empty;
        //        try
        //        {
        //            username = (new VerifyUser()).verifyTokenUser(User);
        //            hierarchy = (new UserDao()).ReadUserHierarchy(username);
        //        }
        //        catch (Exception ex)
        //        {
        //            response.success = false;
        //            response.messages.Add(ex.Message);
        //            return response;
        //        }

        //        RouteData consola = new RouteData();
        //        consola.IdRoute = request.IdRoute;
        //        consola.Device = request.Device;
        //        consola.Date = request.Date;

        //        try
        //        {
        //            response = (new RouteDao()).CreateConsola(consola, hierarchy);
        //        }
        //        catch (Exception ex)
        //        {
        //            response.success = false;
        //            response.messages.Add(ex.Message);
        //            return response;
        //        }

        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        response.success = false;
        //        response.messages.Add(ex.Message);
        //        return response;
        //    }
        //}


    }
}
