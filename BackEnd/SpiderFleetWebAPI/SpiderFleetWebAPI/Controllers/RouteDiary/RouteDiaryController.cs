using SpiderFleetWebAPI.Models.Request.RouteDiary;
using SpiderFleetWebAPI.Models.Response.RouteDiary;
using SpiderFleetWebAPI.Utils.RouteDiary;
using SpiderFleetWebAPI.Utils.User;
using SpiderFleetWebAPI.Utils.VerifyUser;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.RouteDiary
{
    public class RouteDiaryController : ApiController
    {
        private const string Tag = "Agenda de Horarios de Rutas";
        private const string BasicRoute = "api/";
        private const string ResourceName = "configuration/routes/diary";

        [Authorize]
        [HttpPost]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(RouteDiaryResponse))]
        public RouteDiaryResponse Post([FromBody] RouteDiaryRequest diaryRequest)
        {
            RouteDiaryResponse response = new RouteDiaryResponse();

            try
            {
                if (!(diaryRequest is RouteDiaryRequest))
                {
                    response.success = false;
                    response.messages.Add("Objeto de entrada invalido");
                    return response;
                }

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

                CredencialSpiderFleet.Models.RouteDiary.RouteDiary diary = new CredencialSpiderFleet.Models.RouteDiary.RouteDiary();
                diary.Node = hierarchy;
                diary.StartDate = diaryRequest.StartDate;
                diary.EndDate = diaryRequest.EndDate;
                diary.Notes = diaryRequest.Notes;
                diary.Route = diaryRequest.IdRoute;
                diary.Device = diaryRequest.Device;
                diary.Frecuency = diaryRequest.Frecuency;

                try
                {
                    response = (new RouteDiaryDao()).Create(diary);
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
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(RouteDiaryUpdateRequest))]
        public RouteDiaryResponse Put([FromBody] RouteDiaryUpdateRequest diaryRequest)
        {
            RouteDiaryResponse response = new RouteDiaryResponse();
            try
            {

                if (!(diaryRequest is RouteDiaryUpdateRequest))
                {
                    response.success = false;
                    response.messages.Add("Objeto de entrada invalido");
                    return response;
                }

                string hierarchy = string.Empty;

                try
                {
                    string username = string.Empty;
                    username = (new VerifyUser()).verifyTokenUser(User);
                }
                catch (Exception ex)
                {
                    response.success = false;
                    response.messages.Add(ex.Message);
                    return response;
                }

                CredencialSpiderFleet.Models.RouteDiary.RouteDiaryRegistry diary = new CredencialSpiderFleet.Models.RouteDiary.RouteDiaryRegistry();

                diary.IdStart = diaryRequest.IdStart;
                diary.StartDate = diaryRequest.StartDate;
                diary.IdEnd = diaryRequest.IdEnd;
                diary.EndDate = diaryRequest.EndDate;
                diary.Notes = diaryRequest.Notes;
                diary.Route = diaryRequest.IdRoute;
                diary.Device = diaryRequest.Device;

                try
                {
                    response = (new RouteDiaryDao()).Update(diary);
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
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(RouteDiaryListResponse))]
        public RouteDiaryListResponse GetListEvents([FromUri] DateTime startdate, [FromUri] DateTime enddate)
        {
            RouteDiaryListResponse response = new RouteDiaryListResponse();

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
                try
                {
                    response = (new RouteDiaryDao()).Read(startdate, enddate, hierarchy);
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
        [HttpDelete]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(RouteDiaryResponse))]
        public RouteDiaryResponse Delete([FromUri(Name = "start")] int start, [FromUri(Name = "end")] int end)
        {
            RouteDiaryResponse response = new RouteDiaryResponse();

            try
            {

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
                    response = (new RouteDiaryDao()).Delete(start, end);
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
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ListRouteDataResponse))]
        public ListRouteDataResponse GetListRoutes()
        {
            ListRouteDataResponse response = new ListRouteDataResponse();

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
                try
                {
                    response = (new RouteDiaryDao()).GetMongoData(hierarchy);
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
