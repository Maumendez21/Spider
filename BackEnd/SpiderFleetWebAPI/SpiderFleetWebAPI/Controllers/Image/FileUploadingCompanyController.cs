using SpiderFleetWebAPI.Models.Response.Image;
using SpiderFleetWebAPI.Utils.Image;
using SpiderFleetWebAPI.Utils.User;
using SpiderFleetWebAPI.Utils.VerifyUser;
using Swashbuckle.Swagger.Annotations;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Image
{
    public class FileUploadingCompanyController : ApiController
    {
        private const string Tag = "Mantenimiento de Imagen Compañia";
        private const string BasicRoute = "api/";
        private const string ResourceName = "administration/image/companies";

        /// <summary>
        /// Registra una imagen en base 64 con el Id de la Compañia como parametro
        /// </summary>
        /// <remarks>
        /// Este EndPoint obtiene un registro
        /// <param name="id">Id de la Compañia</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ImageResponse))]
        public async Task<ImageResponse> CreateUploadFileCompany() //[FromUri(Name = "company")] int company)
        {
            ImageResponse response = new ImageResponse();

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

            var ctx = HttpContext.Current;
            var root = ctx.Server.MapPath("~/App_Data");
            var provider = new MultipartFormDataStreamProvider(root);

            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);
                string baseImage = string.Empty;

                foreach (var file in provider.FileData)
                {
                    var name = file.Headers
                            .ContentDisposition
                            .FileName;

                    //remove double quotes from string
                    name = name.Trim('"');
                    var localFileName = file.LocalFileName;

                    MemoryStream tempStream = new MemoryStream();
                    using (FileStream source = File.Open(localFileName, FileMode.Open))
                    {
                        source.CopyTo(tempStream);
                        baseImage = Convert.ToBase64String(tempStream.ToArray());
                    }
                }

                CredencialSpiderFleet.Models.Image.Image imagen = new CredencialSpiderFleet.Models.Image.Image();
                imagen.IdCompany = hierarchy;
                imagen.Images = baseImage;

                try
                {
                    response = (new ImageDao()).CreateCompany(imagen);
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

        /// <summary>
        /// Actualiza una imagen en base 64 con el Id de la Imagen como parametro
        /// </summary>
        /// <remarks>
        /// Este EndPoint obtiene un registro
        /// <param name="id">Id de la Imagen</param>
        /// <returns></returns>
        [Authorize]
        [HttpPut]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ImageResponse))]
        public async Task<ImageResponse> UpdateUploadFile([FromUri(Name = "id")] int id)
        {
            ImageResponse response = new ImageResponse();

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

            var ctx = HttpContext.Current;
            var root = ctx.Server.MapPath("~/App_Data");
            var provider = new MultipartFormDataStreamProvider(root);

            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);
                string baseImage = string.Empty;

                foreach (var file in provider.FileData)
                {
                    var name = file.Headers
                            .ContentDisposition
                            .FileName;

                    //remove double quotes from string
                    name = name.Trim('"');
                    var localFileName = file.LocalFileName;

                    MemoryStream tempStream = new MemoryStream();
                    using (FileStream source = File.Open(localFileName, FileMode.Open))
                    {
                        source.CopyTo(tempStream);
                        baseImage = Convert.ToBase64String(tempStream.ToArray());
                    }
                }

                CredencialSpiderFleet.Models.Image.Image imagen = new CredencialSpiderFleet.Models.Image.Image();
                imagen.IdImagen = id;
                imagen.Images = baseImage;

                try
                {
                    response = (new ImageDao()).Update(imagen);
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
