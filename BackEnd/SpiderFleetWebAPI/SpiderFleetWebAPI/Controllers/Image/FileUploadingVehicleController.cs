using SpiderFleetWebAPI.Models.Response.Image;
using SpiderFleetWebAPI.Utils.Image;
using SpiderFleetWebAPI.Utils.VerifyUser;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.Image
{
    public class FileUploadingVehicleController : ApiController
    {
        private const string Tag = "Mantenimiento de Imagen Vehiculos";
        private const string BasicRoute = "api/";
        private const string ResourceName = "administration/image/vehiculo";

        /// <summary>
        /// Registra una imagen en base 64 con el Id del Usuario como parametro
        /// </summary>
        /// <remarks>
        /// Este EndPoint obtiene un registro
        /// <param name="id">Id del Usuario</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route(BasicRoute + ResourceName)]
        [SwaggerOperation(Tags = new[] { Tag })]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ImageResponse))]
        public async Task<ImageResponse> CreateUploadFileCompany([FromUri(Name = "device")] string device)
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
                    break;
                }

                CredencialSpiderFleet.Models.Image.Image imagen = new CredencialSpiderFleet.Models.Image.Image();
                imagen.Devcie = device;
                imagen.Images = baseImage;

                try
                {
                    response = (new ImageDao()).CreateVehicle(imagen);
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
