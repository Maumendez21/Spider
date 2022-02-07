using CredencialSpiderFleet.Models.DAO.CatalogStatusDevice;
using CredencialSpiderFleet.Models.Request.CatalogStatusDevice;
using CredencialSpiderFleet.Models.Response.CatalogStatusDevice;
using CredencialSpiderFleet.Models.Useful;
using System;
using System.Web.Http;

namespace SpiderFleetWebAPI.Controllers.CatalogStatusDevice
{
    public class CatalogStatusDeviceController : ApiController
    {
        private UseFul use = new UseFul();
        private const string Tag = "Mantenimiento de Tipo de Dispositivos";
        private const string BasicRoute = "api/";
        private const string ResourceName = "warehouse";

        /// <summary>
        /// Alta de Roles
        /// </summary>
        /// <remarks>
        /// Este EndPoint nos genera el registro de la comañia
        /// #### Ejemplo de entrada
        /// ##### Busca los datos de un vehiculo con ID XXXX
        /// ```
        /// {
        /// "idsuscriptiontype" : 3
        /// }
        /// ```
        /// </remarks>
        /// <example>
        /// ```
        /// {
        /// "idVehiculo" : "string"
        /// }
        /// ```
        /// </example>
        /// <param name="typeRequest">Objeto de entrada para el Endpoint</param>
        /// <returns>Es un objeto de Company estructura similar a la tabla Companies</returns>
        [HttpPost]
        [Route(BasicRoute + ResourceName + "/catalogstatusdevice")]
        //[SwaggerOperation(Tags = new[] { Tag })]
        //[SwaggerResponse(HttpStatusCode.OK, Type = typeof(GenericResponse))]
        public CatalogStatusDeviceResponse Create([FromBody] CatalogStatusDeviceRequest typeRequest)
        {
            CatalogStatusDeviceResponse response = new CatalogStatusDeviceResponse();

            try
            {
                if (!(typeRequest is CatalogStatusDeviceRequest))
                {
                    response.Success = false;
                    response.Messages.Add("Objeto de entrada invalido");
                    return response;
                }

                try
                {
                    if (string.IsNullOrEmpty(typeRequest.Name.Trim()))
                    {
                        response.Success = false;
                        response.Messages.Add("Ingrese el Nombre ");
                        return response;
                    }
                }
                catch (Exception ex)
                {
                    response.Success = false;
                    response.Messages.Add(ex.Message);
                    return response;
                }

                CatalogStatusDeviceDao dao = new CatalogStatusDeviceDao();
                try
                {
                    int respuesta = dao.Create(typeRequest);
                    if (respuesta == 3)
                    {
                        response.Success = false;
                        response.Messages.Add("Error al intenar dar de alta el registro");
                        return response;
                    }
                    else if (respuesta == 1)
                    {
                        response.Success = true;
                    }
                }
                catch (Exception ex)
                {
                    response.Success = false;
                    response.Messages.Add(ex.Message);
                    return response;
                }

                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Messages.Add(ex.Message);
                return response;
            }
        }

        [HttpPut]
        [Route(BasicRoute + ResourceName + "/catalogstatusdevice")]
        //[SwaggerOperation(Tags = new[] { Tag })]
        //[SwaggerResponse(HttpStatusCode.OK, Type = typeof(CatalogStatusDeviceResponse))]
        public CatalogStatusDeviceResponse Update([FromBody] CatalogStatusDeviceRequest typeRequest)
        {
            CatalogStatusDeviceResponse response = new CatalogStatusDeviceResponse();
            try
            {
                if (!(typeRequest is CatalogStatusDeviceRequest))
                {
                    response.Success = false;
                    response.Messages.Add("Objeto de entrada invalido");
                    return response;
                }

                try
                {
                    if (typeRequest.IdStatus == 0)
                    {
                        response.Success = false;
                        response.Messages.Add("No tiene el parametro Id");
                        return response;
                    }

                    if (string.IsNullOrEmpty(typeRequest.Name.Trim()))
                    {
                        response.Success = false;
                        response.Messages.Add("Ingrese el Nombre ");
                        return response;
                    }
                }
                catch (Exception ex)
                {
                    response.Success = false;
                    response.Messages.Add(ex.Message);
                    return response;
                }

                CatalogStatusDeviceDao dao = new CatalogStatusDeviceDao();
                try
                {
                    int respuesta = dao.Update(typeRequest);

                    if (respuesta == 1)
                    {
                        response.Success = true;
                    }
                    else if (respuesta == 2)
                    {
                        response.Success = false;
                        response.Messages.Add("No se encuentra el registro");
                        return response;
                    }
                    else if (respuesta == 3)
                    {
                        response.Success = false;
                        response.Messages.Add("Error al tratar de actualizar el registro");
                        return response;
                    }
                }
                catch (Exception ex)
                {
                    response.Success = false;
                    response.Messages.Add(ex.Message);
                    return response;
                }

                return response;

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Messages.Add(ex.Message);
                return response;
            }
        }

        [HttpGet]
        [Route(BasicRoute + ResourceName + "/catalogstatusdevice")]
        //[SwaggerOperation(Tags = new[] { Tag })]
        //[SwaggerResponse(HttpStatusCode.OK, Type = typeof(CatalogStatusDeviceResponse))]
        public CatalogStatusDeviceResponse GetListAsync()
        {
            CatalogStatusDeviceResponse response = new CatalogStatusDeviceResponse();
            try
            {
                CatalogStatusDeviceDao dao = new CatalogStatusDeviceDao();

                try
                {
                    response.listStatus = dao.Read();
                    response.Success = true;
                }
                catch (Exception ex)
                {
                    response.Success = false;
                    response.Messages.Add(ex.Message);
                    return response;
                }

                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Messages.Add(ex.Message);
                return response;
            }
        }

        [HttpGet]
        [Route(BasicRoute + ResourceName + "/catalogstatusdevice/{id}")]
        //[SwaggerOperation(Tags = new[] { Tag })]
        //[SwaggerResponse(HttpStatusCode.OK, Type = typeof(CatalogStatusDeviceResponse))]
        public CatalogStatusDeviceResponse GetListIdAsync(int id)
        {
            CatalogStatusDeviceResponse response = new CatalogStatusDeviceResponse();
            try
            {
                CatalogStatusDeviceDao dao = new CatalogStatusDeviceDao();

                try
                {
                    response.listStatus = dao.ReadId(id);
                    response.Success = true;
                }
                catch (Exception ex)
                {
                    response.Success = false;
                    response.Messages.Add(ex.Message);
                    return response;
                }

                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Messages.Add(ex.Message);
                return response;
            }
        }
    }
}

