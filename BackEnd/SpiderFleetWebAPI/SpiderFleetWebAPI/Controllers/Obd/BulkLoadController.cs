using SpiderFleetWebAPI.Models.Response.Obd;
using SpiderFleetWebAPI.Utils.Obd;
using SpiderFleetWebAPI.Utils.VerifyUser;
using Swashbuckle.Swagger.Annotations;
using System;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Obd
{
    public class BulkLoadController : ApiController
    {

        private const string Tag = "Carga Masiva";
        private const string BasicRoute = "api/";
        private const string ResourceName = "bulk/load";

        [Authorize]
        [HttpPost]
        [Route(BasicRoute + ResourceName + "/obds")]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(BulkLoadResponse))]
        public BulkLoadResponse Create([FromUri(Name = "empresa")] string empresa)
        {
            BulkLoadResponse response = new BulkLoadResponse();

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

            try
            {

                var fileCarga = HttpContext.Current.Request.Files.Count > 0 ? HttpContext.Current.Request.Files[0] : null;
                if (fileCarga != null & fileCarga.ContentLength > 0)
                {
                    StreamReader reader = new StreamReader(fileCarga.InputStream);
                    response = (new BulkLoadDao()).BulkLoadObds(reader, empresa);
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
