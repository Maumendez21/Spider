using ExcelDataReader;
using SpiderFleetWebAPI.Models.Response.Route;
using SpiderFleetWebAPI.Utils.Route;
using SpiderFleetWebAPI.Utils.User;
using SpiderFleetWebAPI.Utils.VerifyUser;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Route
{
    public class BulkLoadingRoutesController : ApiController
    {
        private const string Tag = "Carga Masiva Rutas";
        private const string BasicRoute = "api/";
        private const string ResourceName = "bulk/load/routes";

        [Authorize]
        [HttpPost]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(BulkLoadingRoutesResponse))]
        public BulkLoadingRoutesResponse Create()
        {
            BulkLoadingRoutesResponse response = new BulkLoadingRoutesResponse();
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


                var fileCarga = HttpContext.Current.Request.Files.Count > 0 ? HttpContext.Current.Request.Files[0] : null;
                if (fileCarga != null & fileCarga.ContentLength > 0)
                {
                    string name = Path.GetFileNameWithoutExtension(fileCarga.FileName);
                    StreamReader reader = new StreamReader(fileCarga.InputStream);
                    response = (new BulkLoadingRoutesDao()).ReaderExcel(reader, name, hierarchy);
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
