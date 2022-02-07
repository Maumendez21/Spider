using ExcelDataReader;
using SpiderFleetWebAPI.Models.Response.PointsInterest;
using SpiderFleetWebAPI.Utils.PointInterest;
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

namespace SpiderFleetWebAPI.Controllers.PointInterest
{
    public class BulkLoadingPointInterestController : ApiController
    {
        private const string Tag = "Carga Masiva de Punto de Interes";
        private const string BasicRoute = "api/";
        private const string ResourceName = "bulk/load/point/interest";

        [Authorize]
        [HttpPost]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(BulkLoadingPointIntrerestResponse))]
        public BulkLoadingPointIntrerestResponse Create()
        {
            BulkLoadingPointIntrerestResponse response = new BulkLoadingPointIntrerestResponse();

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

                HttpResponseMessage ResponseMessage = null;
                var httpRequest = HttpContext.Current.Request;
                DataSet dsexcelRecords = new DataSet();
                IExcelDataReader reader = null;
                HttpPostedFile Inputfile = null;
                Stream FileStream = null;


                if (httpRequest.Files.Count > 0)
                {
                    Inputfile = httpRequest.Files[0];
                    FileStream = Inputfile.InputStream;

                    if (Inputfile != null && FileStream != null)
                    {
                        if (Inputfile.FileName.EndsWith(".xls"))
                            reader = ExcelReaderFactory.CreateBinaryReader(FileStream);
                        else if (Inputfile.FileName.EndsWith(".xlsx"))
                            reader = ExcelReaderFactory.CreateOpenXmlReader(FileStream);
                        else
                        {
                            response.success = false;
                            response.messages.Add("El archivo no tiene el formato correcto, favor de verificar");
                            return response;
                        }

                        dsexcelRecords = reader.AsDataSet();
                        reader.Close();

                        if (dsexcelRecords != null && dsexcelRecords.Tables.Count > 0)
                        {
                            DataTable dtPuntosInt = dsexcelRecords.Tables[0];
                            //DataTable dtAsignacion = dsexcelRecords.Tables[1];
                            response = (new BulkLoadingPointInterestDao()).ReaderExcel(hierarchy, dtPuntosInt);
                        }
                        else
                        {
                            response.success = false;
                            response.messages.Add("Selecciono un archivo Vacio, favor de verificar");
                            return response;
                        }
                    }
                    else
                    {
                        response.success = false;
                        response.messages.Add("Archivo Invalido");
                        return response;
                    }                        
                }
                else
                {
                    response.success = false;
                    response.messages.Add("Algo salio mal");
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

