using SpiderFleetWebAPI.Models.Request.Diary;
using SpiderFleetWebAPI.Models.Response.Diary;
using SpiderFleetWebAPI.Utils.Diary;
using SpiderFleetWebAPI.Utils.User;
using SpiderFleetWebAPI.Utils.VerifyUser;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Net;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Diary
{
    public class DiaryController : ApiController
    {

        private const string Tag = "Agenda de Horarios de Dispositivos";
        private const string BasicRoute = "api/";
        private const string ResourceName = "configuration/diary";

        [Authorize]
        [HttpPost]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(DiaryResponse))]
        public DiaryResponse Create([FromBody] DiaryRequest diaryRequest)
        {
            DiaryResponse response = new DiaryResponse();

            try
            {
                if (!(diaryRequest is DiaryRequest))
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

                CredencialSpiderFleet.Models.Diary.Diary diary = new CredencialSpiderFleet.Models.Diary.Diary();
                diary.Node = hierarchy;
                diary.StartDate = diaryRequest.StartDate;
                diary.EndDate = diaryRequest.EndDate;
                diary.Notes = diaryRequest.Notes;
                diary.Responsable = diaryRequest.Responsable;
                diary.Device = diaryRequest.Device;
                diary.Frecuency = diaryRequest.Frecuency;

                try
                {
                    response = (new DiaryDao()).Create(diary);
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
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(DiaryResponse))]
        public DiaryResponse Update([FromBody] DiaryUpdateRequest diaryRequest)
        {
            DiaryResponse response = new DiaryResponse();
            try
            {

                if (!(diaryRequest is DiaryUpdateRequest))
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

                CredencialSpiderFleet.Models.Diary.DiaryRegistry diary = new CredencialSpiderFleet.Models.Diary.DiaryRegistry();

                diary.IdStart = diaryRequest.IdStart;
                diary.StartDate = diaryRequest.StartDate;
                diary.IdEnd = diaryRequest.IdEnd;
                diary.EndDate = diaryRequest.EndDate;
                diary.Notes = diaryRequest.Notes;
                diary.Responsable = diaryRequest.Responsable;
                diary.Device = diaryRequest.Device;

                try
                {
                    response = (new DiaryDao()).Update(diary);
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
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(DiaryListResponse))]
        public DiaryListResponse ListEvents([FromUri] DateTime startdate, [FromUri] DateTime enddate)
        {
            DiaryListResponse response = new DiaryListResponse();

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
                    response = (new DiaryDao()).Read(startdate, enddate, hierarchy);
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
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(DiaryResponse))]
        public DiaryResponse Delete([FromUri(Name = "start")] int start, [FromUri(Name = "end")] int end)
        {
            DiaryResponse response = new DiaryResponse();

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
                    response = (new DiaryDao()).Delete(start, end);
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
