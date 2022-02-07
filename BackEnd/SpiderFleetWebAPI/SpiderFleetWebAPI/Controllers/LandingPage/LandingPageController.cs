using SpiderFleetWebAPI.Models.Request.LandingPage;
using SpiderFleetWebAPI.Models.Response.LandingPage;
using SpiderFleetWebAPI.Utils.LandingPage;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.LandingPage
{
    public class LandingPageController : ApiController
    {
        private const string Tag = "Envio de Email LandingPage";
        private const string BasicRoute = "api/";
        private const string ResourceName = "landing/page";

        [HttpPost]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(LandingPageResponse))]
        public LandingPageResponse Create([FromBody] LandingPageRequest request)
        {
            LandingPageResponse response = new LandingPageResponse();

            try
            {
                CredencialSpiderFleet.Models.LandingPage.LandingPage landing = new CredencialSpiderFleet.Models.LandingPage.LandingPage();
                landing.Name = request.Name;
                landing.LastName = request.LastName;
                landing.Phone = request.Phone;
                landing.Email = request.Email;
                landing.Subject = request.Subject;
                landing.Comments = request.Comments;
                landing.Company = request.Company;

                response = (new LandingPageDao()).SendEmail(landing);

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
