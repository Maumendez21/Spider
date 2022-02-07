using CredencialSpiderFleet.Models.Models.Mongo.GeoFence;
using SpiderFleetWebAPI.Models.Request.PointsInterest;
using SpiderFleetWebAPI.Models.Response.PointsInterest;
using SpiderFleetWebAPI.Utils.PointInterest;
using SpiderFleetWebAPI.Utils.User;
using SpiderFleetWebAPI.Utils.VerifyUser;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.PointInterest
{
    public class PointInterestController : ApiController
    {

        private const string Tag = "Punto de Interes";
        private const string BasicRoute = "api/";
        private const string ResourceName = "point/interest";

        [Authorize]
        [HttpPost]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(PointInterestRequest))]
        public PointsInterestResponse Post(
           [FromBody] PointInterestRequest request)
        {
            PointsInterestResponse response = new PointsInterestResponse();

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
                List<double> listMain = new List<double>();
                InterestPoint points = new InterestPoint();

                listMain.Add(Convert.ToDouble(request.Longitude));
                listMain.Add(Convert.ToDouble(request.Latitude));                

                points.Type = "Point";
                points.Coordinate = listMain;
                points.Radius = request.Radius;


                Models.Mongo.PointInterest.PointsInterest point = new Models.Mongo.PointInterest.PointsInterest();
                
                point.Name = request.Name;
                point.Hierarchy = hierarchy;
                point.Active = request.Active;
                point.Description = request.Description;
                point.InterestPoint = points;

                response = (new PointInterestDao()).Create(point, hierarchy);
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
        [HttpPut]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(PointsInterestResponse))]
        public PointsInterestResponse Put(
           [FromBody] PointInterestIdRequest request)
        {
            PointsInterestResponse response = new PointsInterestResponse();

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
                List<double> listMain = new List<double>();
                InterestPoint points = new InterestPoint();
                
                listMain.Add(Convert.ToDouble(request.Longitude));
                listMain.Add(Convert.ToDouble(request.Latitude));

                points.Type = "Point";
                points.Coordinate = listMain;
                points.Radius = request.Radius;

                Models.Mongo.PointInterest.PointsInterest point = new Models.Mongo.PointInterest.PointsInterest();
                point.Id = request.Id;
                point.Name = request.Name;
                //geoFences.Hierarchy = request.Hierarchy;
                point.Active = request.Active;
                point.Description = request.Description;
                point.InterestPoint = points;

                response = (new PointInterestDao()).Update(point, hierarchy);
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
        [Route(BasicRoute + ResourceName + "/list")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(PointInerestListResponse))]
        public PointInerestListResponse GetList()
        {
            PointInerestListResponse response = new PointInerestListResponse();

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
                response = (new PointInterestDao()).Read(hierarchy);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }

            return response;
        }

        [HttpGet]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(PointInterestRegistryResponse))]
        public PointInterestRegistryResponse GetId([FromUri(Name = "id")] string id)
        {
            PointInterestRegistryResponse response = new PointInterestRegistryResponse();

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
                response = (new PointInterestDao()).ReadId(id);
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
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(PointInterestDeleteResponse))]
        public PointInterestDeleteResponse Delete([FromUri(Name = "id")] string id)
        {
            PointInterestDeleteResponse response = new PointInterestDeleteResponse();

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
                response = (new PointInterestDao()).Delete(id);
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
        [Route(BasicRoute + ResourceName + "/list/service")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(PointInterestListRegistryResponse))]
        public PointInterestListRegistryResponse GetListPointInterest()
        {
            PointInterestListRegistryResponse response = new PointInterestListRegistryResponse();

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
                response = (new PointInterestDao()).ListPointsByHierarchyService(hierarchy);
            }
            catch (Exception ex)
            {
                response.success = false;
                response.messages.Add(ex.Message);
                return response;
            }

            return response;
        }

    }
}
